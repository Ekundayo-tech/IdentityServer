using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MabelApi.Contracts.V1
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Candidate
        {
            public const string GetAll = Base + "/posts";
            public const string Create = Base + "/posts"; 
            public const string Delete = Base + "/posts/{id}"; 
        }
        public static class Auth
        {
            public const string Login = Base + "/login";
            public const string Register = Base + "/register"; 
        }

    }
}
