﻿using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DominicanBanking.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection service) 
        {
            service.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            service.AddTransient(typeof(IGenericServices<,,>), typeof(GenericServices<,,>));
            service.AddTransient<IUserServices, UserServices>();
            service.AddTransient<IUserProductServices, UserProductServices>();
            service.AddTransient<IProductServices, ProductServices>();
            service.AddTransient<IPaymentServices, PaymentServices>();
            service.AddTransient<IBeneficiaryServices, BeneficiaryServices>();
            service.AddTransient<ICashAdvanceServices, CashAdvanceServices>();
            service.AddTransient<ITransferServices, TransferServices>();
            #endregion

        }
    }
}
