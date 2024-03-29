﻿using DominicanBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Core.Application.Interfaces.Repository
{
    public interface IUserProductRepository : IGenericRepository<UserProduct>
    {
        Task<List<UserProduct>> GetIncludeAsync();
    }
}
