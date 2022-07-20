using AutoMapper;
using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Enums;
using DominicanBanking.Core.Application.Helpers;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.User;
using DominicanBanking.Core.Application.ViewModel.UserProduct;
using DominicanBanking.WebApp.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DominicanBanking.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IUserProductServices _userProductServices;
        private readonly IMapper _mapper;
        public UserController(IUserServices services,RoleManager<IdentityRole> roleManager, ValidateUserSession validateUserSession, IUserProductServices userProductServices,IMapper mapper)
        {
            _userServices = services;
            _roleManager = roleManager;
            _validateUserSession = validateUserSession;
            _userProductServices = userProductServices;
            _mapper = mapper;
        }

        public IActionResult Login()
        {
            if (_validateUserSession.IsLogin())
            {
               var user = HttpContext.Session.Get<AuthenticationResponse>("user");
                return RedirectToRoute(new { action = user.Roles.Any(r=>r=="ADMINISTRATOR")?"DashBoard":"Client", controller = "Home" });
            }

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
        [Authorize(Roles="ADMINISTRATOR")]
        public async Task<IActionResult> UserList() {
            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            return View(await _userServices.GetAllUserAsync());

        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Register()
        {
            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            ViewBag.Roles = await _roleManager.Roles.ToListAsync();

            return View(new SaveUserViewModel());

        }

        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Activate(string id) {

            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            var response = await _userServices.GetAllUserAsync();
            var user = response.FirstOrDefault(user => user.Id == id);

            if (user == null) {

                return RedirectToAction("UserList");
            }

            return View(user);
        
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Activate(UserViewModel user) {

            ActivateViewModel activate = new() { UserId = user.Id };

            await _userServices.ActivateAsync(activate);

            return RedirectToAction("UserList");

        }
        
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Deactivate(string id) {

            if (!_validateUserSession.IsLogin())
            {
                return RedirectToRoute(new { action = "Login", controller = "User" });
            }

            var response = await _userServices.GetAllUserAsync();
            var user = response.FirstOrDefault(user => user.Id == id);

            if (user == null) {

                return RedirectToAction("UserList");
            }

            return View(user);
        
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Deactivate(UserViewModel user) {

            ActivateViewModel activate = new() { UserId = user.Id };

            await _userServices.DeactivateAsync(activate);

            return RedirectToAction("UserList");

        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Register(SaveUserViewModel vm) {

            if (!ModelState.IsValid) {
                ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                return View(vm);
            
            }
            if (vm.UserType == Roles.ADMINISTRATOR.ToString())
            {
                var response = await _userServices.RegisterAdministratorAsync(vm);

                if (response.HasError)
                {
                    ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                    vm.HasError = response.HasError;
                    vm.Error = response.Error;
                    return View(vm);
                }

                return RedirectToAction("UserList");
            }
            else
            {
                var response = await _userServices.RegisterClientAsync(vm);

                if (response.HasError) {
                    ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                    vm.HasError = response.HasError;
                    vm.Error = response.Error;
                    return View(vm);
                }

                return RedirectToAction("UserList");
            }
        }
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(string id)
        {
            var userWithoutFilter = await _userServices.GetAllUserAsync();

            var userFiltered = userWithoutFilter.FirstOrDefault(x => x.Id == id);

            return View(new SaveEditViewModel()
            {
                Id = userFiltered.Id,
                Documents = userFiltered.Documents,
                Email = userFiltered.Email,
                UserType = userFiltered.Roles.FirstOrDefault(),
                LastName =userFiltered.LastName,
                Name = userFiltered.FirstName,
                Username = userFiltered.UserName,
                Phone =userFiltered.Phone
            });

        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> EditClient(SaveEditViewModel vm) {

            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            if (!string.IsNullOrEmpty(vm.Password))
            {
                PasswordRequest request = new() {UserId=vm.Id,NewPassword=vm.Password };

                var response = await _userServices.ChangePasswordAsync(request);

                if (response.HasError)
                {
                    vm.HasError = response.HasError;
                    vm.Error = response.Error;
                    return View("Edit", vm);
                }
            }

            var product = await _userProductServices.GetAllViewModelWithIncludes();

            var Principal = product.FirstOrDefault(x => x.UserId == vm.Id && x.IsPrincipal == true);
            Principal.Amount += vm.Amount.Value;

            await _userServices.EditUserAsync(vm);
            await _userProductServices.Update(_mapper.Map<SaveUserProductViewModel>(Principal),Principal.Id);

            return RedirectToRoute(new { action = "UserList", controller = "User" });

        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> EditAdministrator(SaveEditViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            if (!string.IsNullOrEmpty(vm.Password))
            {
                PasswordRequest request = new() { UserId = vm.Id, NewPassword = vm.Password };

                var response = await _userServices.ChangePasswordAsync(request);

                if (response.HasError)
                {
                    vm.HasError = response.HasError;
                    vm.Error = response.Error;
                    return View("Edit", vm);
                }
            }

            await _userServices.EditUserAsync(vm);

            return RedirectToRoute(new { action = "UserList", controller = "User" });

        }

    }
}
