using ECommerceAPI.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ECommerceAPI.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool tracking);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking);
        Task<T> GetByIdAsync(string id, bool tracking);
    }
}
