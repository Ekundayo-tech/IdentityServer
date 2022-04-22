using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MabelApi.Contracts.Response
{
    public class RegisterResponse
    { 
        public string Token { get; set; }
        public string UserId { get; set; }
    }
     
    public class RegFailureResponse
    {
        public IEnumerable<string> Error { get; set; }
    }
}
