using DominicanBanking.Core.Application.ViewModel.CashAdvance;
using DominicanBanking.Core.Domain.Entities;

namespace DominicanBanking.Core.Application.Interfaces.Services
{
    public interface ICashAdvanceServices : IGenericServices<SaveCashAdvanceViewModel,CashAdvanceViewModel,CashAdvance>
    {

    }
}
