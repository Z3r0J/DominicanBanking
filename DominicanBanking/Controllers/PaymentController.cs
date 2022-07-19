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
        private readonly IBeneficiaryServices _beneficiaryServices;
        public PaymentController(IPaymentServices paymentServices, IUserProductServices userProductServices, IUserServices userServices,IBeneficiaryServices beneficiaryServices)
        {
            _paymentServices = paymentServices;
            _userProductServices = userProductServices;
            _userServices = userServices;
            _beneficiaryServices = beneficiaryServices;
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

                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Available: ${x.Amount}" }), "Value", "Text");

                return View(model);
            }
            if (model.IdentifyNumberFrom == model.IdentifyNumberTo)
            {
                ModelState.AddModelError("IdentifyNumberTo", "The account number is the same, please write another account");
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

        public async Task<IActionResult> CreditCard()
        {
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");

            ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");
            ViewBag.CreditCard = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 2).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Dues: ${x.Amount}" }), "Value", "Text");

            return View(new SavePaymentViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreditCard(SavePaymentViewModel model) {

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();

            if (!ModelState.IsValid)
            {

                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");
                ViewBag.CreditCard = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 2).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Dues: ${x.Amount}" }), "Value", "Text");

                return View(model);
            }
            var CreditCard = allProduct.FirstOrDefault(x=>x.IdentifyNumber == model.IdentifyNumberTo);
            var Account = allProduct.FirstOrDefault(x=>x.IdentifyNumber == model.IdentifyNumberFrom);

            if (CreditCard.Amount == 0) {

                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");
                ViewBag.CreditCard = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 2).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Dues: ${x.Amount}" }), "Value", "Text");

                ModelState.AddModelError("IdentifyNumberTo", "This credit card hasn't debts. Select another oner");
                return View(model);
            }

            if (model.Amount>Account.Amount)
            {

                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Available: ${x.Amount}" }), "Value", "Text");
                ViewBag.CreditCard = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 2).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} - Dues: ${x.Amount}" }), "Value", "Text");

                ModelState.AddModelError("Amount", "The Amount is greater than your have in that account.");
                return View(model);
            }

            if (model.Amount>CreditCard.Amount)
            {
                Account.Amount -= model.Amount;
                CreditCard.Amount = model.Amount -= CreditCard.Amount;
                Account.Amount += CreditCard.Amount;
                CreditCard.Amount -=CreditCard.Amount;

                var SaveFrom = new SaveUserProductViewModel()
                {
                    Id = Account.Id,
                    IdentifyNumber = Account.IdentifyNumber,
                    Amount = Account.Amount,
                    IsPrincipal = Account.IsPrincipal,
                    Limit = Account.Limit,
                    ProductId = Account.ProductId,
                    UserId = Account.UserId
                };

                var SaveTo = new SaveUserProductViewModel()
                {
                    Id = CreditCard.Id,
                    IdentifyNumber = CreditCard.IdentifyNumber,
                    Amount = CreditCard.Amount,
                    IsPrincipal = CreditCard.IsPrincipal,
                    Limit = CreditCard.Limit,
                    ProductId = CreditCard.ProductId,
                    UserId = CreditCard.UserId
                };

                model.UserId = userLog.Id;
                model.TypeId = 2;

                await _userProductServices.Update(SaveFrom, SaveFrom.Id);
                await _userProductServices.Update(SaveTo, SaveTo.Id);
                await _paymentServices.Add(model);
            }
            else
            {
                Account.Amount -= model.Amount;
                CreditCard.Amount -= model.Amount;

                var SaveFrom = new SaveUserProductViewModel()
                {
                    Id = Account.Id,
                    IdentifyNumber = Account.IdentifyNumber,
                    Amount = Account.Amount,
                    IsPrincipal = Account.IsPrincipal,
                    Limit = Account.Limit,
                    ProductId = Account.ProductId,
                    UserId = Account.UserId
                };

                var SaveTo = new SaveUserProductViewModel()
                {
                    Id = CreditCard.Id,
                    IdentifyNumber = CreditCard.IdentifyNumber,
                    Amount = CreditCard.Amount,
                    IsPrincipal = CreditCard.IsPrincipal,
                    Limit = CreditCard.Limit,
                    ProductId = CreditCard.ProductId,
                    UserId = CreditCard.UserId
                };

                model.UserId = userLog.Id;
                model.TypeId = 2;

                await _userProductServices.Update(SaveFrom, SaveFrom.Id);
                await _userProductServices.Update(SaveTo, SaveTo.Id);
                await _paymentServices.Add(model);
            }

            return RedirectToRoute(new {action="Client",controller="Home" });
        }

        public async Task<IActionResult> Beneficiary() {
            
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();
            var allBeneficiary = await _beneficiaryServices.GetAllViewModel();

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");

            ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Available: ${x.Amount}" }), "Value", "Text");
            ViewBag.Beneficiary = new SelectList(allBeneficiary.Where(x => x.UserId == userLog.Id).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.Name} {x.LastName} | {x.IdentifyNumber}" }), "Value", "Text");

            return View(new SavePaymentViewModel());
        
        }
        [HttpPost]
        public async Task<IActionResult> Beneficiary(SavePaymentViewModel model) {

            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();
            var allBeneficiary = await _beneficiaryServices.GetAllViewModel();

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");

            if (!ModelState.IsValid) {

                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Available: ${x.Amount}" }), "Value", "Text");
                ViewBag.Beneficiary = new SelectList(allBeneficiary.Where(x => x.UserId == userLog.Id).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.Name} {x.LastName} | {x.IdentifyNumber}" }), "Value", "Text");

                return View(model);

            }
            var AccountFrom = allProduct.FirstOrDefault(r => r.IdentifyNumber == model.IdentifyNumberFrom);
            var AccountTo = allProduct.FirstOrDefault(r => r.IdentifyNumber == model.IdentifyNumberTo);

            if (model.Amount>AccountFrom.Amount)
            {
                ModelState.AddModelError("error", "The Amount is greater than your have in that account.");


                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Available: ${x.Amount}" }), "Value", "Text");
                ViewBag.Beneficiary = new SelectList(allBeneficiary.Where(x => x.UserId == userLog.Id).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.Name} {x.LastName} | {x.IdentifyNumber}" }), "Value", "Text");
                return View(model);
            }

            var user = await _userServices.GetAllUserAsync();
            model.TypeId = 4;

            return View("QuestionExpress", new QuestionViewModel()
            {
                FullName = $"{user.FirstOrDefault(user => user.Id == AccountTo.UserId).FirstName} {user.FirstOrDefault(user => user.Id == AccountTo.UserId).LastName}",
                IdentifyNumberTo = model.IdentifyNumberTo,
                IdentifyNumberFrom = model.IdentifyNumberFrom,
                Amount = model.Amount,
                TypePaymentId = model.TypeId,
                UserId = AccountFrom.UserId
            });

        }
    }
}
