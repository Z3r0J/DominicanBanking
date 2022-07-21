using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.DashBoard;
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
        private readonly ITransferServices _transferServices;
        private readonly IPaymentServices _paymentServices;
        private readonly IUserServices _userServices;

        public HomeController(IUserProductServices userProductServices, ValidateUserSession validateUserSession, IUserServices userServices,ITransferServices transferServices,IPaymentServices paymentServices)
        {
            _userProductServices = userProductServices;
            _validateUserSession = validateUserSession;
            _userServices = userServices;
            _transferServices = transferServices;
            _paymentServices = paymentServices;
        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Dashboard()
        {
            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            var AllTransfer = await _transferServices.GetAllViewModel();
            var AllPayment = await _paymentServices.GetAllViewModel();
            var AllClient = await _userServices.GetAllUserAsync();
            var AllProduct = await _userProductServices.GetAllViewModelWithIncludes();

            DashboardViewModel Dvm = new() {

                Transaction = AllTransfer.Count,
                TransactionToday = AllTransfer.Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Count(),
                PaymentToday = AllPayment.Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Count(),
                PaymentTotal = AllPayment.Count,
                ClientActive = AllClient.Where(x => x.Roles.Any(x => x == "CLIENT") && x.IsVerified == true).Count(),
                ClientInactive = AllClient.Where(x => x.Roles.Any(x => x == "CLIENT") && x.IsVerified == false).Count(),
                ProductQuantityCredit = AllProduct.Where(x => x.ProductId == 2).Count(),
                ProductQuantitySavings = AllProduct.Where(x=>x.ProductId==1).Count(),
                ProductQuantityLoan = AllProduct.Where(x => x.ProductId==3).Count(),
                ProductQuantityTotal = AllProduct.Count
            };

            return View(Dvm);
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
