using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MabelApi.Contracts.Requests
{
    public class UserRegister
    { 
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
