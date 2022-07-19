using AutoMapper;
using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.CashAdvance;
using DominicanBanking.Core.Domain.Entities;

namespace DominicanBanking.Core.Application.Services
{
    public class CashAdvanceServices : GenericServices<SaveCashAdvanceViewModel, CashAdvanceViewModel, CashAdvance>, ICashAdvanceServices
    {
        private readonly ICashAdvanceRepository _cashAdvanceRepository;
        private readonly IMapper _mapper;

        public CashAdvanceServices(ICashAdvanceRepository cashAdvanceRepository, IMapper mapper): base(cashAdvanceRepository, mapper)
        {
            _cashAdvanceRepository = cashAdvanceRepository;
            _mapper = mapper;
        }
    }
}
