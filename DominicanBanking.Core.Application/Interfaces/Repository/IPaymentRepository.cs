using DominicanBanking.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Interfaces.Repository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
      Task<List<Payment>> GetIncludeAsync();
    }
}