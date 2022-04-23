using System.Collections.Generic;

namespace MabelApi.Interface
{
    public class AuthenticationResult
    { 
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessage { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
    }
}