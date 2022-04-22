using MabelApi.Data;
using MabelApi.Interface;
using MabelApi.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MabelApi.Services
{
    public class AuthService: IAuth
    {
        private readonly UserManager<IdentityUser> _user;
        private readonly JwtOptions _jwt;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;

        public AuthService(UserManager<IdentityUser> usermanager, JwtOptions jwt, TokenValidationParameters tokenValidationParameters, DataContext context)
        {
            _user = usermanager;
            _jwt = jwt;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }

        public async Task<AuthenticationResult> Login(string email, string password)
        {
            var user = await _user.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] { "user does not exist" }
                };
            }

            var userHasValidPassword = await _user.CheckPasswordAsync(user, password);
            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] { "user or password combination is wrong" }
                };
            }

            return await GenerateAuthenticResultForUserAsync(user);
             

        }
         

        private async Task<AuthenticationResult> GenerateAuthenticResultForUserAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Secret);
            var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id),
            };
            var userClaims = await _user.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var tokenHandlerDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwt.TokenLifeSpan),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenHandlerDescriptor);
              
            return new AuthenticationResult
            {
                Success = true, 
                Token = tokenHandler.WriteToken(token),
                UserId = user.Id
            };

        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();


            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validateToken);
            if (IsJwtWithValidSecurityAlgo(validateToken))
                return principal;

            return null;

        }

        private bool IsJwtWithValidSecurityAlgo(SecurityToken security)
        {
            return (security is JwtSecurityToken jwtSecurityToken) && (jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<AuthenticationResult> Register(string email, string password)
        {
            var exist = await _user.FindByEmailAsync(email);
            if (exist != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] { "user already exist" }
                };
            }

            var userId = Guid.NewGuid();
            var user = new IdentityUser
            {
                Email = email,
                UserName = email
            }; 
            var createdUser = await _user.CreateAsync(user, password);
            if (!createdUser.Succeeded)
                return new AuthenticationResult
                {
                    ErrorMessage = createdUser.Errors.Select(x => x.Description)
                };

            await _user.AddClaimAsync(user, new Claim("tags.view", "true"));
            return await GenerateAuthenticResultForUserAsync(user);
             

        }

    }
}
