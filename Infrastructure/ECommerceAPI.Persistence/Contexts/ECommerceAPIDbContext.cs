using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Common;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Contexts
{

    //It was derived from DbContext however due to identity mechanism, IdentityDbContext is being used
    public class ECommerceAPIDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        //In order to make some adjustments/options while IoC Container is calling this Context
        public ECommerceAPIDbContext(DbContextOptions options) : base(options)
        {
            //This will be filled in IoC Container
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<InvoiceFile> Invoices { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<CompletedOrder> CompletedOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            

            //Basket can be without order, however order cannot be without basket 
            //So we should define basketId as primaryKey 
            builder.Entity<Order>()
               .HasKey(b => b.Id);
            //OrderCode to be unique
            builder.Entity<Order>()
                .HasIndex(o => o.OrderCode)
                .IsUnique();

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Basket)
                .HasForeignKey<Order>(b => b.Id);

            //one-to-one
            builder.Entity<Order>()
                .HasOne(o => o.CompletedOrder)
                .WithOne(c => c.Order)
                .HasForeignKey<CompletedOrder>(c => c.OrderId);


            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //get all changes made through BaseEntity which makes it generic
            var metadatas = ChangeTracker.Entries<BaseEntity>();

            //checks each data and if there is any modification/insert, it will authomatically add related datas to Entities modified/inserted to db
            foreach (var data in metadatas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
