using DominicanBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DominicanBanking.Infrastructure.Identity.Contexts
{
    public class IdentityContext : IdentityDbContext<BankUsers>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Fluent API
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");

            builder.Entity<BankUsers>(entity => {

                entity.ToTable(name: "Users");
                            
            });

            builder.Entity<IdentityRole>(entity => {

                entity.ToTable(name: "Roles");
                            
            });

            builder.Entity<IdentityUserRole<string>>(entity => {

                entity.ToTable(name: "UserRoles");
                            
            });
            
            builder.Entity<IdentityUserLogin<string>>(entity => {

                entity.ToTable(name: "UserLogins");
                            
            });
        }
    }
}
