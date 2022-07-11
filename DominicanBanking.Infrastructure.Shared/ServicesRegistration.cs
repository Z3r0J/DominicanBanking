using DominicanBanking.Core.Application.Interfaces.Services;
using DominicanBanking.Core.Domain.Settings;
using DominicanBanking.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DominicanBanking.Infrastructure.Shared
{
    public static class ServicesRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {

            service.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            service.AddTransient<IEmailServices, EmailServices>();
        }
    }
}
