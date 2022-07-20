using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.CashAdvance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DominicanBanking.Core.Application.ViewModel.UserProduct;

namespace DominicanBanking.WebApp.Controllers
{
    [Authorize(Roles="CLIENT")]
    public class CashAdvanceController : Controller
    {
        private readonly ICashAdvanceServices _cashAdvanceServices;
        private readonly IUserProductServices _userProductServices;
        private readonly IMapper _mapper;
        public CashAdvanceController(IUserProductServices userProductServices,ICashAdvanceServices cashAdvanceServices,IMapper mapper)
        {
            _userProductServices = userProductServices;
            _cashAdvanceServices = cashAdvanceServices;
            _mapper = mapper;
        }
        public async Task<IActionResult> New()
        {
            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();

            ViewBag.CreditCard = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 2).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Limit: ${x.Limit}" }), "Value", "Text");
            ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | {x.ProductName}" }), "Value", "Text");

            return View(new SaveCashAdvanceViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(SaveCashAdvanceViewModel model) {

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();

            if (!ModelState.IsValid)
            {
                ViewBag.CreditCard = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 2).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Limit: ${x.Limit}" }), "Value", "Text");
                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | {x.ProductName}" }), "Value", "Text");

                return View(model);
            }

            var CreditCard = allProduct.FirstOrDefault(up => up.IdentifyNumber == model.CreditCardNumberFrom);
            var Account = allProduct.FirstOrDefault(up => up.IdentifyNumber == model.IdentifyNumberTo);

            if (model.Amount>CreditCard.Limit)
            {
                ViewBag.CreditCard = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 2).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Limit: ${x.Limit}" }), "Value", "Text");
                ViewBag.Account = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | {x.ProductName}" }), "Value", "Text");

                ModelState.AddModelError("Amount", "The Amount to Advance is exceeding the limit of the credit card.");

                return View(model);
            }

            Account.Amount += model.Amount;
            CreditCard.Amount += 6.25 + model.Amount;


            model.UserId = userLog.Id;

            await _userProductServices.Update(_mapper.Map<SaveUserProductViewModel>(CreditCard), CreditCard.Id);
            await _userProductServices.Update(_mapper.Map<SaveUserProductViewModel>(Account), Account.Id);

            await _cashAdvanceServices.Add(model);

            return RedirectToRoute(new { action="Client",controller="Home"});
        
        }
    }
}
