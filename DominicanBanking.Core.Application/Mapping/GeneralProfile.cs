using AutoMapper;
using DominicanBanking.Core.Application.DTOS.Account;
using DominicanBanking.Core.Application.ViewModel.Beneficiary;
using DominicanBanking.Core.Application.ViewModel.CashAdvance;
using DominicanBanking.Core.Application.ViewModel.Payment;
using DominicanBanking.Core.Application.ViewModel.Product;
using DominicanBanking.Core.Application.ViewModel.Transfer;
using DominicanBanking.Core.Application.ViewModel.User;
using DominicanBanking.Core.Application.ViewModel.UserProduct;
using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.UserType, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AccountResponse, UserViewModel>()
                .ForMember(x=>x.Roles,opt=>opt.MapFrom(ac=>ac.Roles))
                .ReverseMap();

            CreateMap<UserProduct, SaveUserProductViewModel>()
               .ReverseMap()
               .ForMember(x => x.Product, opt => opt.Ignore())
               .ForMember(x => x.Created, opt => opt.Ignore())
               .ForMember(x => x.CreatedBy, opt => opt.Ignore())
               .ForMember(x => x.Modified, opt => opt.Ignore())
               .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<UserProduct, UserProductViewModel>()
               .ForMember(x => x.ProductName, opt => opt.MapFrom(ac => ac.Product.Name))
               .ReverseMap()
               .ForMember(x => x.Created, opt => opt.Ignore())
               .ForMember(x => x.CreatedBy, opt => opt.Ignore())
               .ForMember(x => x.Modified, opt => opt.Ignore())
               .ForMember(x => x.ModifiedBy, opt => opt.Ignore());
            
            CreateMap<Product, ProductViewModel>()
               .ReverseMap()
               .ForMember(x => x.Created, opt => opt.Ignore())
               .ForMember(x => x.CreatedBy, opt => opt.Ignore())
               .ForMember(x => x.Modified, opt => opt.Ignore())
               .ForMember(x => x.ModifiedBy, opt => opt.Ignore());
            
            CreateMap<Payment, SavePaymentViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.TypePayment, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());
            
            CreateMap<Beneficiary, BeneficiaryViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());
            
            CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<CashAdvance, CashAdvanceViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<Beneficiary, SaveCashAdvanceViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<Transfer, TransferViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<Beneficiary, SaveTransferViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<ActivateRequest, ActivateViewModel>()
                .ReverseMap(); 
        }
       
    }
}
