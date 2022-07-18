using AutoMapper;
using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.ViewModel.Product;
using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Services
{
    public class ProductServices : GenericServices<SaveProductViewModel ,ProductViewModel,Product>,IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductServices(IProductRepository productRepository,IMapper mapper) : base(productRepository,mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
    }
}
