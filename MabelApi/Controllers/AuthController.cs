using MabelApi.Contracts.Requests;
using MabelApi.Contracts.Response;
using MabelApi.Contracts.V1;
using MabelApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace TweetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;

        public AuthController(IAuth auth)
        {
            _auth = auth;
        }


        [HttpPost(ApiRoutes.Auth.Register)]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResult
                {
                    ErrorMessage = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            var res = await _auth.Register(userRegister.Email, userRegister.Password);
            if (!res.Success)
            {
                return BadRequest(
                    new RegFailureResponse
                    {
                        Error = res.ErrorMessage
                    }
                );
            }

            return Ok(new RegisterResponse
            {
                Token = res.Token,
                UserId = res.UserId
            });
        }

        [HttpPost(ApiRoutes.Auth.Login)]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var res = await _auth.Login(userLogin.Email, userLogin.Password);
            if (!res.Success)
            {
                return BadRequest(
                    new RegFailureResponse
                    {
                        Error = res.ErrorMessage
                    }
                );
            }

            return Ok(new RegisterResponse
            {
                Token = res.Token,
                UserId = res.UserId
            });
        }
         
    }
}
