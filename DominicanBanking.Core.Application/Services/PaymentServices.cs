using AutoMapper;
using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.Payment;
using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Services
{
    public class PaymentServices : GenericServices<SavePaymentViewModel,PaymentViewModel,Payment>,IPaymentServices
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        public PaymentServices(IPaymentRepository paymentRepository, IMapper mapper) : base(paymentRepository,mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
        public async Task<List<PaymentViewModel>> GetAllViewModelWithIncludes()
        {

            var response = await _paymentRepository.GetIncludeAsync();

            return _mapper.Map<List<PaymentViewModel>>(response);

        }
    }
}
