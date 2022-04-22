using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MabelApi.Interface
{
    public interface IAuth
    { 
        Task<AuthenticationResult> Register(string email, string password);
        Task<AuthenticationResult> Login(string email, string password); 
    }
}
