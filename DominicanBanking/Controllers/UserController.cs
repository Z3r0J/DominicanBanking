using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.User;
using DominicanBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DominicanBanking.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices services)
        {
            _userServices = services;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm) {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var response = await _userServices.LoginAsync(vm);

            if (response != null && response.HasError!= true) {

                HttpContext.Session.Set<AuthenticationResponse>("user", response);

                return response.Roles.Any(x=>x=="ADMINISTRATOR") ? RedirectToRoute(new { action="Dashboard",Controller="Home"}) : RedirectToRoute(new { action = "Client", Controller = "Home" });
            }
            else
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

        }

        public async Task<IActionResult> LogOut() {

            HttpContext.Session.Remove("user");
            await _userServices.LogOutAsync();

            return RedirectToRoute(new { action = "Login", controller = "User" });

        }
    }
}
