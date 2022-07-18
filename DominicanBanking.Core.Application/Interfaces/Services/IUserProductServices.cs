using DominicanBanking.Core.Application.ViewModel.UserProduct;
using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Interfaces.Services
{
    public interface IUserProductServices : IGenericServices<SaveUserProductViewModel,UserProductViewModel,UserProduct>
    {
        Task<List<UserProductViewModel>> GetAllViewModelWithIncludes();
    }
}
