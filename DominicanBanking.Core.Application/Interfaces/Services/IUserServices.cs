using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<AuthenticationResponse> LoginAsync(LoginViewModel model);
        Task<RegisterResponse> RegisterClientAsync(SaveUserViewModel model);
        Task LogOutAsync();
        Task<List<UserViewModel>> GetAllUserAsync();

    }
}
