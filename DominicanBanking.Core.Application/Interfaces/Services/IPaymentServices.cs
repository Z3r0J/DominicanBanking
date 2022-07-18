using DominicanBanking.Core.Application.ViewModel.Payment;
using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentServices : IGenericServices<SavePaymentViewModel,PaymentViewModel,Payment>
    {
        Task<List<PaymentViewModel>> GetAllViewModelWithIncludes();
    }
}
