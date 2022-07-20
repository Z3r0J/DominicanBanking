using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace DominicanBanking.WebApp.Middlewares
{
    public class ValidateUserSession
    {

        private readonly IHttpContextAccessor _httpContext;

        public ValidateUserSession(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool IsLogin() {
            var response = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");

            return response!=null ? true : false;
        
        }
    }
}
