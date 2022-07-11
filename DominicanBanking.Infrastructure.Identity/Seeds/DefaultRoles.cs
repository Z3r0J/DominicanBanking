using DominicanBanking.Core.Application.Enums;
using DominicanBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominicanBanking.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<BankUsers> userManager,RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new(Roles.ADMINISTRATOR.ToString()));            
            await roleManager.CreateAsync(new(Roles.CLIENT.ToString()));


        }
    }
}
