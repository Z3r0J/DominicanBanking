using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Infrastructure.Identity.Services
{
    public class AccountServices
    {

        private readonly UserManager<BankUsers> _userManager;
        private readonly SignInManager<BankUsers> _signInManager;
        private readonly IEmailServices _emailServices;


        public AccountServices(UserManager<BankUsers> userManager, SignInManager<BankUsers> signInManager, IEmailServices emailServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request) {

            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user==null)
            {
                response.HasError = true;
                response.Error = $"The system don't have an account with this email {request.Email}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credential for {request.Email}";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Your Account is disable, Contact an Administrator.";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Id;
            response.UserName = user.Id;

            var RoleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = RoleList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

    }
}
