using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.Payment;
using DominicanBanking.Core.Application.ViewModel.Question;
using DominicanBanking.Core.Application.ViewModel.UserProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace DominicanBanking.WebApp.Controllers
{
    [Authorize(Roles = "CLIENT")]
    public class PaymentController : Controller
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IUserProductServices _userProductServices;
        private readonly IUserServices _userServices;
        public PaymentController(IPaymentServices paymentServices, IUserProductServices userProductServices, IUserServices userServices)
        {
            _paymentServices = paymentServices;
            _userProductServices = userProductServices;
            _userServices = userServices;
        }

        public async Task<IActionResult> Express()
        {
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");

            ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");

            return View(new SavePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Express(SavePaymentViewModel model) {

            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();
            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.IdentifyNumberFrom == model.IdentifyNumberTo)
            {
                ModelState.AddModelError("IdentifyNumberTo", "The account number is the same please write another account");
                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");

                return View(model);
            }

            var AccountFrom = allProduct.FirstOrDefault(r => r.IdentifyNumber == model.IdentifyNumberFrom);
            var AccountTo = allProduct.FirstOrDefault(r => r.IdentifyNumber == model.IdentifyNumberTo);

            if (AccountTo == null
                || AccountTo.ProductId == 2
                || AccountTo.ProductId == 3)
            {
                ModelState.AddModelError("IdentifyNumberTo", "The account number doesn't exist or This account number is an Loan or Credit Card from another person.");
                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");

                return View(model);
            }


            if (model.Amount>AccountFrom.Amount)
            {
                ModelState.AddModelError("Amount", "The Amount is greater than your have in that account.");
                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");

                return View(model);
            }

            var user = await _userServices.GetAllUserAsync();
            model.TypeId = 1;

            return View("QuestionExpress",new QuestionViewModel() {
                FullName=$"{user.FirstOrDefault(user => user.Id == AccountTo.UserId).FirstName} {user.FirstOrDefault(user => user.Id == AccountTo.UserId).LastName}",
                IdentifyNumberTo = model.IdentifyNumberTo,
                IdentifyNumberFrom = model.IdentifyNumberFrom,
                Amount = model.Amount,
                TypePaymentId = model.TypeId,
                UserId = AccountFrom.UserId});
        }

        [HttpPost]
        public async Task<IActionResult> QuestionExpress(QuestionViewModel model) {

            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();
            var AccountFrom = allProduct.FirstOrDefault(r => r.IdentifyNumber == model.IdentifyNumberFrom);
            var AccountTo = allProduct.FirstOrDefault(r => r.IdentifyNumber == model.IdentifyNumberTo);

            AccountFrom.Amount -= model.Amount;
            AccountTo.Amount += model.Amount;

            var SaveFrom = new SaveUserProductViewModel()
            {
                Id = AccountFrom.Id,
                IdentifyNumber = AccountFrom.IdentifyNumber,
                Amount = AccountFrom.Amount,
                IsPrincipal = AccountFrom.IsPrincipal,
                Limit = AccountFrom.Limit,
                ProductId = AccountFrom.ProductId,
                UserId = AccountFrom.UserId
            };

            var SaveTo = new SaveUserProductViewModel()
            {
                Id= AccountTo.Id,
                IdentifyNumber = AccountTo.IdentifyNumber,
                Amount = AccountTo.Amount,
                IsPrincipal = AccountTo.IsPrincipal,
                Limit = AccountTo.Limit,
                ProductId = AccountTo.ProductId,
                UserId = AccountTo.UserId
            };

            var payment = new SavePaymentViewModel() { 
             IdentifyNumberFrom = model.IdentifyNumberFrom,
             IdentifyNumberTo = model.IdentifyNumberTo,
             Amount = model.Amount,
             TypeId = model.TypePaymentId,
             UserId = model.UserId            
            };

            await _userProductServices.Update(SaveFrom,AccountFrom.Id);
            await _userProductServices.Update(SaveTo, AccountTo.Id);
            await _paymentServices.Add(payment);


            return RedirectToRoute(new {action="Client",controller="Home" });
        
        }

        public async Task<IActionResult> Beneficiary() {

            return View();
        
        }
    }
}
