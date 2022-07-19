﻿using AutoMapper;
using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.Beneficiary;
using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Services
{
    public class BeneficiaryServices : GenericServices<SaveBeneficiaryViewModel, BeneficiaryViewModel,Beneficiary>, IBeneficiaryServices
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IMapper _mapper;

        public BeneficiaryServices(IBeneficiaryRepository beneficiaryRepository, IMapper mapper) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _mapper = mapper;
        }
    }
}
