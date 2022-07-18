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
    }
}
