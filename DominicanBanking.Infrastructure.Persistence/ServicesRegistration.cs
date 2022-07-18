using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Infrastructure.Persistence.Contexts;
using DominicanBanking.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DominicanBanking.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {

        public static void AddPersistenceInfrastructure(this IServiceCollection service, IConfiguration configuration) {

            #region Contexts

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                service.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("DominicanBankingMemory"));
            }
            else
            {
                service.AddDbContext<ApplicationContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DominicanBankingConnection"), 
                m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }

            #endregion

            #region Repository

            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddTransient<IUserProductRepository, UserProductRepository>();
            service.AddTransient<IProductRepository, ProductRepository>();
            service.AddTransient<IPaymentRepository, PaymentRepository>();

            #endregion
        }

    }
}
