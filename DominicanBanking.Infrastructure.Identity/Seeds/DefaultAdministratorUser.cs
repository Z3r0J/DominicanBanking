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
    public class DefaultAdministratorUser
    {
        public static async Task SeedAsync(UserManager<BankUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            BankUsers adminUser = new() {
                UserName = "DefaultAdmin",
                Name = "Jean",
                LastName = "Reyes",
                Email = "jreyes@dominicanbanking.com.do",
                EmailConfirmed = true,
                PhoneNumber="+1 809 935 0913",
                PhoneNumberConfirmed = true
            };

            BankUsers adminUser1 = new()
            {
                UserName = "DefaultJoseMiguel",
                Name = "Jose Miguel",
                LastName = "Cayetano",
                Email = "jmcayetano@dominicanbanking.com.do",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            
            BankUsers adminUser2 = new()
            {
                UserName = "DefaultJohanly",
                Name = "Johanly",
                LastName = "Baez",
                Email = "jlbaez@dominicanbanking.com.do",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u=>u.Id!=adminUser.Id))
            {
                var user = await userManager.FindByEmailAsync(adminUser.Email);

                if (user==null)
                {
                    await userManager.CreateAsync(adminUser,"P@ssw0rd");

                    await userManager.AddToRoleAsync(adminUser,Roles.ADMINISTRATOR.ToString());
                }
            }
            
            if (userManager.Users.All(u=>u.Id!=adminUser1.Id))
            {
                var user = await userManager.FindByEmailAsync(adminUser1.Email);

                if (user==null)
                {
                    await userManager.CreateAsync(adminUser1,"P@ssw0rd");

                    await userManager.AddToRoleAsync(adminUser1,Roles.ADMINISTRATOR.ToString());
                }
            }

            if (userManager.Users.All(u => u.Id != adminUser2.Id))
            {
                var user = await userManager.FindByEmailAsync(adminUser2.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(adminUser2, "P@ssw0rd");

                    await userManager.AddToRoleAsync(adminUser2, Roles.ADMINISTRATOR.ToString());
                }
            }
        }
    }
}
