using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.Transfer;
using DominicanBanking.Core.Domain.Entities;

namespace DominicanBanking.Core.Application.Services
{
    public class TransferServices : GenericServices<SaveTransferViewModel, TransferViewModel, Transfer>, ITransferServices
    {
        public readonly ITransferRepository _transferRepository;
        public readonly IMapper _mapper;

        public TransferServices(ITransferRepository transferRepository, IMapper mapper) : base(transferRepository, mapper)
        {
            _transferRepository = transferRepository;
            _mapper = mapper;
        }
    }
}
