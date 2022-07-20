using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Models;
using DominicanBanking.WebApp.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DominicanBanking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProductServices _userProductServices;
        private readonly ValidateUserSession _validateUserSession;
        public HomeController(IUserProductServices userProductServices, ValidateUserSession validateUserSession)
        {
            _userProductServices = userProductServices;
            _validateUserSession = validateUserSession;
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Dashboard()
        {
            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            return View();
        }

        [Authorize(Roles = "CLIENT")]
        public async Task<IActionResult> Client()
        {
            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            var info = HttpContext.Session.Get<AuthenticationResponse>("user");

            var UserProduct = await _userProductServices.GetAllViewModelWithIncludes();



            return View(UserProduct.Where(up=>up.UserId==info.Id).ToList());
        }
    }
}
