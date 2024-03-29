﻿using AutoMapper;
using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;

        public UserServices(IAccountServices accountServices, IMapper mapper)
        {
            _accountServices = accountServices;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel model) {

            AuthenticationRequest request = _mapper.Map<AuthenticationRequest>(model);

            AuthenticationResponse response = await _accountServices.AuthenticateAsync(request);

            return response;
        }

        public async Task<PasswordResponse> ChangePasswordAsync(PasswordRequest password) {

            PasswordResponse response = await _accountServices.ChangePasswordAsync(password);

            return response;

        }

        public async Task<EditResponse> EditUserAsync(SaveEditViewModel vm) {
            EditRequest request = _mapper.Map<EditRequest>(vm);

            EditResponse response = await _accountServices.EditAccountAsync(request);

            return response;
        }

        public async Task<RegisterResponse> RegisterClientAsync(SaveUserViewModel model) { 
            
            RegisterRequest request = _mapper.Map<RegisterRequest>(model);

            return await _accountServices.RegisterClientUserAsync(request);

        }

        public async Task<RegisterResponse> RegisterAdministratorAsync(SaveUserViewModel model) {

            RegisterRequest request = _mapper.Map<RegisterRequest>(model);

            return await _accountServices.RegisterAdministratorAsync(request);

        }

        public async Task<List<UserViewModel>> GetAllUserAsync() {

            var response = await _accountServices.GetUsersAsync();

            return _mapper.Map<List<UserViewModel>>(response);
        
        }

        public async Task<ActivateResponse> ActivateAsync(ActivateViewModel model) {

           return await _accountServices.ActivateAsync(_mapper.Map<ActivateRequest>(model));

        }
        public async Task<ActivateResponse> DeactivateAsync(ActivateViewModel model) {

           return await _accountServices.DeactivateAsync(_mapper.Map<ActivateRequest>(model));

        }

        public async Task LogOutAsync() {
            await _accountServices.LogOutAsync();    

        }
    }
}
