using DominicanBanking.Core.Application.DTOS.Account;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Interfaces.Services
{
    public interface IAccountServices
    {
        Task<ActivateResponse> ActivateAsync(ActivateRequest request);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<ActivateResponse> DeactivateAsync(ActivateRequest request);
        Task LogOutAsync();
        Task<RegisterResponse> RegisterAdministratorAsync(RegisterRequest request);
        Task<RegisterResponse> RegisterClientUserAsync(RegisterRequest request);
    }
}