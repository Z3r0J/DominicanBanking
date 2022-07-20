using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.Beneficiary;
using DominicanBanking.WebApp.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DominicanBanking.WebApp.Controllers
{
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryServices _beneficiaryServices;
        private readonly IUserProductServices _userProductServices;
        private readonly IUserServices _userServices;
        private readonly ValidateUserSession _validateUserSession;

        public BeneficiaryController(IBeneficiaryServices beneficiaryServices, IUserProductServices userProductServices,IUserServices userServices,ValidateUserSession validateUserSession)
        {
            _beneficiaryServices = beneficiaryServices;
            _userProductServices = userProductServices;
            _userServices = userServices;
            _validateUserSession = validateUserSession;
        }
        [Authorize(Roles = "CLIENT")]
        public  async Task<IActionResult> Index()
        {
            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            var beneficiaryWithoutFiltered = await _beneficiaryServices.GetAllViewModel();
            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");

            return View(beneficiaryWithoutFiltered.Where(b=>b.UserId == userLog.Id).ToList());
        }
        [Authorize(Roles = "CLIENT")]
        [HttpPost]
        public async Task<IActionResult> Index(string IdentifyNumber) {

            var beneficiaryWithoutFiltered = await _beneficiaryServices.GetAllViewModel();

            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");

            var account = await _userProductServices.GetAllViewModelWithIncludes();

            var account_Info = account.FirstOrDefault(x => x.IdentifyNumber == IdentifyNumber&&x.ProductId==1);

            if (account_Info == null)
            {
                ModelState.AddModelError("error", "This account number doesn't exist or This account number is a Loan or a Credit Card from another person");
                return View(beneficiaryWithoutFiltered.Where(b => b.UserId == userLog.Id).ToList());
            }

            if (beneficiaryWithoutFiltered.Where(b=>b.UserId==userLog.Id).FirstOrDefault(x=>x.IdentifyNumber==IdentifyNumber)!=null)
            {
                ModelState.AddModelError("error", "This account is your beneficiary already.");
                return View(beneficiaryWithoutFiltered.Where(b => b.UserId == userLog.Id).ToList());
            }

            if (account.FirstOrDefault(x=>x.IdentifyNumber==IdentifyNumber&&x.UserId==userLog.Id)!=null)
            {
                ModelState.AddModelError("error", "You cannot add you as a Beneficiary.");
                return View(beneficiaryWithoutFiltered.Where(b => b.UserId == userLog.Id).ToList());
            }

            var user = await _userServices.GetAllUserAsync();

            var beneficiary = new SaveBeneficiaryViewModel()
            {
                IdentifyNumber = IdentifyNumber,
                Name = user.FirstOrDefault(x=>x.Id == account_Info.UserId).FirstName,
                LastName = user.FirstOrDefault(x => x.Id == account_Info.UserId).LastName,
                UserId = userLog.Id
            };

            await _beneficiaryServices.Add(beneficiary);

            return RedirectToRoute(new {action="Index",controller="Beneficiary" });

        }

        public async Task<IActionResult> Delete(int id) {

            var beneficiary = await _beneficiaryServices.GetByIdSaveViewModel(id);
            var userLog = HttpContext.Session.Get<AuthenticationResponse>("user");

            if (beneficiary.UserId!=userLog.Id)
            {
                return RedirectToRoute(new { action = "Index", controller = "Beneficiary" });
            }


            return View(beneficiary);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(SaveBeneficiaryViewModel model) {

            await _beneficiaryServices.Delete(model.Id);

            return RedirectToRoute(new { action = "Index", controller = "Beneficiary" });
        
        }
    }
}
