using DominicanBanking.Core.Domain.Common;
using DominicanBanking.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DominicanBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }
        public DbSet<Beneficiary> Beneficiary { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TypePayment> TypePayments { get; set; }
        public DbSet<CashAdvance> Advances { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.Modified = DateTime.Now;
                        entry.Entity.ModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }



        protected override void OnModelCreating(ModelBuilder builder) {
            //Fluent API

            builder.HasDefaultSchema("DominicanBanking");
            #region Tables

            builder.Entity<Product>().ToTable("Products");
            builder.Entity<UserProduct>().ToTable("User_Products");
            builder.Entity<Beneficiary>().ToTable("Beneficiary");
            builder.Entity<Payment>().ToTable("Payments");
            builder.Entity<TypePayment>().ToTable("Type_Payments");
            builder.Entity<CashAdvance>().ToTable("Advances");
            builder.Entity<Transfer>().ToTable("Transfers");

            #endregion

            #region Primary Key

            builder.Entity<Product>().HasKey(product=>product.Id);
            builder.Entity<UserProduct>().HasKey(userproduct=>userproduct.Id);
            builder.Entity<Beneficiary>().HasKey(beneficiary=>beneficiary.Id);
            builder.Entity<Payment>().HasKey(payment=>payment.Id);
            builder.Entity<TypePayment>().HasKey(typepayment=>typepayment.Id);
            builder.Entity<CashAdvance>().HasKey(advances=>advances.Id);
            builder.Entity<Transfer>().HasKey(transfer=>transfer.Id);

            #endregion

            #region RelationShips


            builder.Entity<Product>()
                .HasMany(product => product.UserProducts)
                .WithOne(userproduct => userproduct.Product)
                .HasForeignKey(userproduct => userproduct.ProductId);

            builder.Entity<TypePayment>()
                .HasMany(type => type.Payments)
                .WithOne(payment => payment.TypePayment)
                .HasForeignKey(payment =>payment.TypeId);

            #endregion

            #region Seeds

            builder.Entity<Product>().HasData(
                new() {Id=1,Name="Savings Account", CreatedBy="DefaultDatabaseAdministrator",Created=DateTime.Now },
                new() {Id=2,Name="Credit Card", CreatedBy = "DefaultDatabaseAdministrator", Created = DateTime.Now }, 
                new() {Id=3,Name="Loan", CreatedBy = "DefaultDatabaseAdministrator", Created = DateTime.Now });

            builder.Entity<TypePayment>().HasData(
                new() {Id=1,Name="Express Payment", CreatedBy="DefaultDatabaseAdministrator",Created=DateTime.Now },
                new() {Id=2,Name="Credit Card Payment", CreatedBy = "DefaultDatabaseAdministrator", Created = DateTime.Now }, 
                new() {Id=3,Name="Loan Payment", CreatedBy = "DefaultDatabaseAdministrator", Created = DateTime.Now },  
                new() {Id=4,Name="Beneficiary Payment", CreatedBy = "DefaultDatabaseAdministrator", Created = DateTime.Now });

            #endregion

            #region Properties

            #region User_Product

            builder.Entity<UserProduct>()
                .Property(x => x.IdentifyNumber)
                .IsRequired();

            builder.Entity<UserProduct>()
                .Property(x => x.Amount)
                .IsRequired();

            builder.Entity<UserProduct>()
                .Property(x => x.UserId)
                .IsRequired();
            #endregion

            #region Payment

            builder.Entity<Payment>()
                .Property(x => x.IdentifyNumberFrom)
                .IsRequired();

            builder.Entity<Payment>()
                .Property(x => x.Amount)
                .IsRequired();

            builder.Entity<Payment>()
                .Property(x => x.IdentifyNumberTo)
                .IsRequired();

            builder.Entity<Payment>()
                .Property(x => x.UserId)
                .IsRequired();

            #endregion

            #region Beneficiary
            
            builder.Entity<Beneficiary>()
                .Property(x => x.IdentifyNumber)
                .IsRequired();
            
            builder.Entity<Beneficiary>()
                .Property(x => x.Name)
                .IsRequired();
            
            builder.Entity<Beneficiary>()
                .Property(x => x.LastName)
                .IsRequired();
            
            builder.Entity<Beneficiary>()
                .Property(x => x.UserId)
                .IsRequired();

            #endregion

            #region CashAdvance
            
            builder.Entity<CashAdvance>()
                .Property(x => x.CreditCardNumberFrom)
                .IsRequired();
            
            builder.Entity<CashAdvance>()
                .Property(x => x.Amount)
                .IsRequired();
            
            builder.Entity<CashAdvance>()
                .Property(x => x.IdentifyNumberTo)
                .IsRequired();
            
            builder.Entity<CashAdvance>()
                .Property(x => x.UserId)
                .IsRequired();

            #endregion

            #region Transfer
            
            builder.Entity<Transfer>()
                .Property(x => x.IdentifyNumberFrom)
                .IsRequired();
            
            builder.Entity<Transfer>()
                .Property(x => x.Amount)
                .IsRequired();
            
            builder.Entity<Transfer>()
                .Property(x => x.IdentifyNumberTo)
                .IsRequired();
            
            builder.Entity<Transfer>()
                .Property(x => x.UserId)
                .IsRequired();

            #endregion

            #endregion

        }
    }
}
