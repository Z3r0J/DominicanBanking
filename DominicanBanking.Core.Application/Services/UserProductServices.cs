using AutoMapper;
using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.UserProduct;
using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Services
{
    public class UserProductServices : GenericServices<SaveUserProductViewModel ,UserProductViewModel,UserProduct>,IUserProductServices
    {
        private readonly IUserProductRepository _productRepository;
        private readonly IMapper _mapper;
        public UserProductServices(IUserProductRepository productRepository,IMapper mapper) : base(productRepository,mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
    }
}
