using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.Beneficiary;
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

        public BeneficiaryController(IBeneficiaryServices beneficiaryServices, IUserProductServices userProductServices,IUserServices userServices)
        {
            _beneficiaryServices = beneficiaryServices;
            _userProductServices = userProductServices;
            _userServices = userServices;
        }
        [Authorize(Roles = "CLIENT")]
        public  async Task<IActionResult> Index()
        {
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
    }
}
