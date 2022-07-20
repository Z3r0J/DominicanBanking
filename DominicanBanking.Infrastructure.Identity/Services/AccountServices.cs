using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Enums;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Infrastructure.Identity.Services
{
    public class AccountServices : IAccountServices
    {

        private readonly UserManager<BankUsers> _userManager;
        private readonly SignInManager<BankUsers> _signInManager;
        private readonly IUserProductServices _userProductServices;


        public AccountServices(UserManager<BankUsers> userManager, SignInManager<BankUsers> signInManager,IUserProductServices userProductServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userProductServices = userProductServices;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {

            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"You don't have an account with this email {request.Email}";
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
            response.FirstName = user.Name;
            response.LastName = user.LastName;
            response.Documents = user.Documents;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var RoleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = RoleList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task<List<AccountResponse>> GetUsersAsync() {

            List<AccountResponse> accounts = new();

            var response = await _userManager.Users.ToListAsync();

            foreach (BankUsers user in response)
            {
               var roles = await _userManager.GetRolesAsync(user);

                AccountResponse account = new() { 
                Id = user.Id,
                FirstName=user.Name,
                LastName=user.LastName,
                Documents = user.Documents,
                Email = user.Email,
                Roles = roles.ToList(),
                UserName=user.UserName,
                IsVerified = user.EmailConfirmed
                };

                accounts.Add(account);
            }

            return accounts;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<RegisterResponse> RegisterClientUserAsync(RegisterRequest request)
        {
            RegisterResponse response = new() { HasError = false };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);

            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"The username {request.Username} has been taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"The Email {request.Email} already registered.";
                return response;
            }

            var user = new BankUsers
            {
                Email = request.Email,
                Name = request.Name,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.Username,
                Documents = request.Documents
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.CLIENT.ToString());
                var userWasRegister = await _userManager.FindByEmailAsync(request.Email);

                await _userProductServices.Add(new()
                {
                    IdentifyNumber = Guid.NewGuid().ToString().Substring(0,9),
                    IsPrincipal = true,
                    Amount = request.Amount.Value,
                    Limit = null,
                    ProductId = 1,
                    UserId = userWasRegister.Id
                });

            }
            else
            {
                response.HasError = true;
                response.Error = $"An Error ocurred, please try again.";
                return response;
            }

            return response;

        }

        public async Task<RegisterResponse> RegisterAdministratorAsync(RegisterRequest request)
        {
            RegisterResponse response = new() { HasError = false };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);

            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"The username {request.Username} has been taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"The Email {request.Email} already registered.";
                return response;
            }

            var user = new BankUsers
            {
                Email = request.Email,
                Name = request.Name,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.Username,
                Documents = request.Documents
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.ADMINISTRATOR.ToString());
            }
            else
            {
                response.HasError = true;
                response.Error = $"An Error ocurred, please try again.";
                return response;
            }

            return response;

        }

        public async Task<ActivateResponse> ActivateAsync(ActivateRequest request)
        {

            ActivateResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"The user with this Id {request.UserId} doesn't exist";
                return response;
            }

            user.EmailConfirmed = true;

            await _userManager.UpdateAsync(user);

            return response;

        }
        public async Task<PasswordResponse> ChangePasswordAsync(PasswordRequest request) {

            var user = await _userManager.FindByIdAsync(request.UserId);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var response = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            PasswordResponse passwordResponse = new();

            if (response.Succeeded)
            {
                passwordResponse.HasError = false;
            }
            if (response.Errors.Count()>0) {
                foreach (var error in response.Errors)
                {
                    passwordResponse.HasError = true;
                    passwordResponse.Error = error.Description;
                }
            }

            return passwordResponse;
        }

        public async Task<EditResponse> EditAccountAsync(EditRequest request) {

            var user = await _userManager.FindByIdAsync(request.Id);

            user.Name = request.Name;
            user.NormalizedEmail = request.Email.ToUpper();
            user.NormalizedUserName = request.Username.ToUpper();
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            user.Documents = request.Documents;
            user.Email = request.Email;
            user.UserName = request.Username;

            var usm = await _userManager.UpdateAsync(user);

            EditResponse editResponse = new();

            if (usm.Succeeded)
            {
                editResponse.HasError = false;
            }
            else
            {
                editResponse.HasError = true;
                foreach (var error in usm.Errors)
                {
                    editResponse.HasError = true;
                    editResponse.Error = error.Description;
                }
            }

            return editResponse;

        }

        public async Task<ActivateResponse> DeactivateAsync(ActivateRequest request)
        {

            ActivateResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"The user with this Id {request.UserId} doesn't exist";
                return response;
            }

            user.EmailConfirmed = false;

            await _userManager.UpdateAsync(user);

            return response;

        }

    }
}
