﻿using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Infrastructure.Identity.Contexts;
using DominicanBanking.Infrastructure.Identity.Entities;
using DominicanBanking.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DominicanBanking.Infrastructure.Identity
{
    public static class ServicesRegistration
    {

        public static void AddIdentityInfrastructure(this IServiceCollection service, IConfiguration configuration) {

            #region Contexts

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                service.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityMemory"));
            }
            else
            {
                service.AddDbContext<IdentityContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"), 
                m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }

            #endregion

            #region Identity
            service.AddIdentity<BankUsers, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            service.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/User/AccessDenied";
                });

            service.AddAuthentication();


            #endregion

            #region Services

            service.AddTransient<IAccountServices, AccountServices>();

            #endregion

        }

    }
}
