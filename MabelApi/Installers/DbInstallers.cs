using MabelApi.Authorization;
using MabelApi.Data;
using MabelApi.Installers;
using MabelApi.Interface;
using MabelApi.Options;
using MabelApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace TweetApi.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {

            var jwtSettings = new JwtOptions();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);


            var tokenValidatorParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
            };

            services.AddSingleton(tokenValidatorParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidatorParameters;
            });


            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "GODP Cloud",
                    Version = "V2",
                    Description = "An API to perform business automated operations",
                    TermsOfService = new Uri("http://www.mabel.co.uk/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Emmanuel Ekundayo",
                        Email = "gbengahe@gmail.com",
                        Url = new Uri("http://www.mabel.co.uk/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Tweeter API LICX",
                        Url = new Uri("http://www.mabel.co.uk/"),
                    },

                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0] }
                };
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Tweeet Cloud Authorization header using bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {new OpenApiSecurityScheme {Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    } }, new List<string>() }
                });
            });

            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
             
            services.AddScoped<IAuth, AuthService>();
            //services.AddMvc(options => {
            //    options.Filters.Add<ValidationFilter>();
            //});

            services.AddAuthorization(options =>
            {
                options.AddPolicy("mabel", policy =>
                {
                    policy.AddRequirements(new AuthorizationRequirement("mabel.com"));
                });
            });
            services.AddSingleton<IAuthorizationHandler, WorkAuthorizationHandlers>();
        }
    }
}
