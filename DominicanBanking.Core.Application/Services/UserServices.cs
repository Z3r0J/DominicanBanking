using AutoMapper;
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

        public async Task<RegisterResponse> RegisterClientAsync(SaveUserViewModel model) { 
            
            RegisterRequest request = _mapper.Map<RegisterRequest>(model);

            return await _accountServices.RegisterClientUserAsync(request);

        }

        public async Task LogOutAsync() {
            await _accountServices.LogOutAsync();    

        }
    }
}
