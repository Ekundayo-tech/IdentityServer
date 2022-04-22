using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MabelApi.Authorization
{

    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public string Domain { get; set; }
        public AuthorizationRequirement(string domainName)
        {
            Domain = domainName;
        }

    }
}
