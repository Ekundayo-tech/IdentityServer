using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MabelApi.Authorization
{
    public class WorkAuthorizationHandlers : AuthorizationHandler<AuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext authorizationHandlerContext, AuthorizationRequirement authorizationRequirement)
        {
            var userEmailAddress = authorizationHandlerContext.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            if (userEmailAddress.EndsWith(authorizationRequirement.Domain))
            {

                authorizationHandlerContext.Succeed(authorizationRequirement);
                return Task.CompletedTask;
            }
            authorizationHandlerContext.Fail();
            return Task.CompletedTask;
        }
    }
}
