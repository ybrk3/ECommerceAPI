using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Contexts
{
    public class ECommerceAPIDbContext : DbContext
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
