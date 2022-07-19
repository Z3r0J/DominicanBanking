using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.UserProduct;
using DominicanBanking.Core.Domain.Entities;
using DominicanBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominicanBanking.WebApp.Controllers
{
    [Authorize]
    public class UserProductController : Controller
    {
        private readonly IUserProductServices _userProduct;
        private readonly IProductServices _productServices;
        private readonly IUserServices _userServices;
        public UserProductController(IUserProductServices userProduct,UserManager<BankUsers> userManager,IProductServices productServices,IUserServices userServices)
        {
            _userProduct = userProduct;
            _productServices = productServices;
            _userServices = userServices;
        }

        [Authorize(Roles="ADMINISTRATOR")]
        public async Task<IActionResult> Index()
        { 
            return View(await _userProduct.GetAllViewModelWithIncludes());
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Create()
        {
            var usersWithoutFilter = await _userServices.GetAllUserAsync();

            var userfiltered = usersWithoutFilter.Where(u => u.Roles.Any(r => r == "CLIENT")).ToList();

            ViewBag.Users = new SelectList(from x in userfiltered select new { Value = x.Id, Text = $"{x.FirstName} {x.LastName}" }, "Value", "Text");

            ViewBag.Types = await _productServices.GetAllViewModel();
            
            return View(new SaveUserProductViewModel());
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Create(SaveUserProductViewModel vm) {

            if (!ModelState.IsValid)
            {

                var usersWithoutFilter = await _userServices.GetAllUserAsync();

                var userfiltered = usersWithoutFilter.Where(u => u.Roles.Any(r => r == "CLIENT")).ToList();

                ViewBag.Users = new SelectList(from x in userfiltered select new { Value = x.Id, Text = $"{x.FirstName} {x.LastName}" }, "Value", "Text");

                ViewBag.Types = await _productServices.GetAllViewModel();
                return View(vm);
            }

            vm.IdentifyNumber = Guid.NewGuid().ToString().Substring(0,9);

            var allAccount = await _userProduct.GetAllViewModelWithIncludes();

            if (allAccount.FirstOrDefault(x=>x.IdentifyNumber==vm.IdentifyNumber)!=null)
            {
                var usersWithoutFilter = await _userServices.GetAllUserAsync();

                var userfiltered = usersWithoutFilter.Where(u => u.Roles.Any(r => r == "CLIENT")).ToList();

                ViewBag.Users = new SelectList(from x in userfiltered select new { Value = x.Id, Text = $"{x.FirstName} {x.LastName}" }, "Value", "Text");
                ViewBag.Types = await _productServices.GetAllViewModel();

                ModelState.AddModelError("error","Something went wrong try again.");
                return View(vm);
            }

            vm.IsPrincipal = false;

            await _userProduct.Add(vm);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {

            var allAccount = await _userProduct.GetAllViewModelWithIncludes();

            var account = allAccount.FirstOrDefault(x=>x.Id==id);

            return View(new SaveUserProductViewModel() {
                Id=account.Id,
                IdentifyNumber=account.IdentifyNumber,
                Amount = account.Amount,
                Limit=account.Limit,
                IsPrincipal =account.IsPrincipal,
                ProductId = account.ProductId,
                UserId =account.UserId});

        }

        [Authorize(Roles = "ADMINISTRATOR")]
        [HttpPost]
        public async Task<IActionResult> Delete(SaveUserProductViewModel model) {

            var allAccount = await _userProduct.GetAllViewModelWithIncludes();
            var account = allAccount.Where(user=>user.UserId == model.UserId).ToList();

            if (model.IsPrincipal)
            {
                return RedirectToRoute(new { action = "Index", controller = "UserProduct" });
            }

            if (model.ProductId == 1)
            {
                var PrincipalAccount = account.FirstOrDefault(x => x.IsPrincipal == true);

                PrincipalAccount.Amount+=model.Amount;

                var SaveAccount = new SaveUserProductViewModel()
                {
                    Id = PrincipalAccount.Id,
                    IdentifyNumber = PrincipalAccount.IdentifyNumber,
                    Amount = PrincipalAccount.Amount,
                    Limit = PrincipalAccount.Limit,
                    IsPrincipal = PrincipalAccount.IsPrincipal,
                    ProductId = PrincipalAccount.ProductId,
                    UserId = PrincipalAccount.UserId
                };

                await _userProduct.Update(SaveAccount, PrincipalAccount.Id);

                await _userProduct.Delete(model.Id);

            }

            if (model.ProductId == 2)
            {
                if (model.Amount>0)
                {
                    ModelState.AddModelError("error", "The Credit Card has debts");
                    return View(model);
                }

                await _userProduct.Delete(model.Id);
            }

            if (model.ProductId == 3)
            {
                if (model.Amount>0)
                {
                    ModelState.AddModelError("error", "The Loan has debts");
                    return View(model);
                }

                await _userProduct.Delete(model.Id);
            }
            return RedirectToRoute(new { action = "Index", controller = "UserProduct" });
        }

    }
}
