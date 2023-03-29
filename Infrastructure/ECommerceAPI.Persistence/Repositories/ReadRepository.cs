using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Domain.Entities.Common;
using ECommerceAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Repositories
{
    //"No-tracking query" option added to increase performance
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ECommerceAPIDbContext _context;
        public ReadRepository(ECommerceAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking=true)
        {
            IQueryable<T> query = Table.AsQueryable<T>();
            if (!tracking)
                query.AsNoTracking();
            return query;
        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            IQueryable<T> query = Table.Where(predicate);
            if (!tracking)
                query.AsNoTracking();
            return query;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            IQueryable<T> query = Table.AsQueryable<T>();
            if (!tracking)
                query.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate);
        }
        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        { 
            IQueryable<T> query=Table.AsQueryable<T>();
            if (!tracking)
                query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));//we reached id property through marker base class which is BaseEntity

        }
    }
}
