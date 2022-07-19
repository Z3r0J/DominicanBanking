using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Models;
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
        public HomeController(IUserProductServices userProductServices)
        {
            _userProductServices = userProductServices;
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "CLIENT")]
        public async Task<IActionResult> Client()
        {
            var info = HttpContext.Session.Get<AuthenticationResponse>("user");

            var UserProduct = await _userProductServices.GetAllViewModelWithIncludes();



            return View(UserProduct.Where(up=>up.UserId==info.Id).ToList());
        }
    }
}
