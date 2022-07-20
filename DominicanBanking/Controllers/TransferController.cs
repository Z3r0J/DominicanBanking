using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DominicanBanking.WebApp.Middlewares;
using DominicanBanking.Core.Application.ViewModel.Transfer;
using DominicanBanking.Core.Application.ViewModel.UserProduct;

namespace DominicanBanking.WebApp.Controllers
{
    [Authorize(Roles="CLIENT")]
    public class TransferController : Controller
    {
        private readonly ITransferServices _transferServices;
        private readonly IUserProductServices _userProductServices;
        private readonly IMapper _mapper;
        private readonly ValidateUserSession _validateUserSession;

        public TransferController(IUserProductServices userProductServices,ITransferServices transferServices,IMapper mapper, ValidateUserSession validateUserSession)
        {
            _userProductServices = userProductServices;
            _transferServices = transferServices;
            _mapper = mapper;
            _validateUserSession = validateUserSession;
        }
        public async Task<IActionResult> Create()
        {
            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }
            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();

            ViewBag.AccountFrom = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Avalaible: ${x.Amount}" }), "Value", "Text");
            ViewBag.AccountTo = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | {x.ProductName}" }), "Value", "Text");

            return View(new SaveTransferViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveTransferViewModel model) {

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");
            var allProduct = await _userProductServices.GetAllViewModelWithIncludes();

            if (!ModelState.IsValid)
            {
                ViewBag.AccountFrom = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Avalaible: ${x.Amount}" }), "Value", "Text");
                ViewBag.AccountTo = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | {x.ProductName}" }), "Value", "Text");

                return View(model);
            }

            var AccountFrom = allProduct.FirstOrDefault(up => up.IdentifyNumber == model.IdentifyNumberFrom);
            var AccountTo = allProduct.FirstOrDefault(up => up.IdentifyNumber == model.IdentifyNumberTo);

            if (AccountFrom.IdentifyNumber == AccountTo.IdentifyNumber)
            {
                ViewBag.AccountFrom = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Avalaible: ${x.Amount}" }), "Value", "Text");
                ViewBag.AccountTo = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | {x.ProductName}" }), "Value", "Text");

                return View(model);
            }

            if (model.Amount>AccountFrom.Amount)
            {
                ViewBag.AccountFrom = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | Avalaible: ${x.Amount}" }), "Value", "Text");
                ViewBag.AccountTo = new SelectList(allProduct.Where(x => x.UserId == userLog.Id && x.ProductId == 1).Select(x => new { Value = x.IdentifyNumber, Text = $"{x.IdentifyNumber} | {x.ProductName}" }), "Value", "Text");
                ModelState.AddModelError("Amount", "The Amount is greater than you have in that account");

                return View(model);
            }

            AccountFrom.Amount -= model.Amount;
            AccountTo.Amount += model.Amount;


            model.UserId = userLog.Id;

            await _userProductServices.Update(_mapper.Map<SaveUserProductViewModel>(AccountFrom), AccountFrom.Id);
            await _userProductServices.Update(_mapper.Map<SaveUserProductViewModel>(AccountTo), AccountTo.Id);

            await _transferServices.Add(model);

            return RedirectToRoute(new { action="Client",controller="Home"});
        
        }
    }
}
