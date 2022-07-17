using DominicanBanking.Core.Application.Interfaces.Repository;
using DominicanBanking.Core.Domain.Entities;
using DominicanBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Infrastructure.Persistence.Repositories
{
    public class UserProductRepository:GenericRepository<UserProduct>, IUserProductRepository
    {
        private readonly ApplicationContext _applicationContext;
        public UserProductRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }
    }
}
