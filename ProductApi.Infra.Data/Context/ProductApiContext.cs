using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infra.Data.Context
{
    public class ProductApiContext : DbContext
    {
        public ProductApiContext(DbContextOptions<ProductApiContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            UpdateDatesEntries();
            return await base.SaveChangesAsync();
        }
        private void UpdateDatesEntries()
        {
            ChangeTracker.Entries().OrderBy(x => x.State);
            IEnumerator<EntityEntry> enumerator = ChangeTracker.Entries().GetEnumerator();
            EntityEntry? current = null;

            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                DealWithEntity(current, current.Entity);
            }
        }

        private void DealWithEntity(EntityEntry entry, object entity)
        {
            bool added = entry.State == EntityState.Added;
            bool modified = entry.State == EntityState.Modified;

            if (!added && !modified)
                return;

            Type type = entity.GetType();

            bool createdAt = type.GetProperty("CreatedAt") != null;
            bool updatedAt = type.GetProperty("UpdatedAt") != null;

            if (!createdAt && !updatedAt)
                return;

            if (added && createdAt)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }
            else if (modified && createdAt)
            {
                entry.Property("CreatedAt").IsModified = false;
            }

            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }
    }
}
