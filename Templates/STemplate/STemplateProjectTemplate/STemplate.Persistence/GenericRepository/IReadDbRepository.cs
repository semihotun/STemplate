using STemplate.Domain.SeedWork;
using System.Linq.Expressions;
namespace STemplate.Persistence.GenericRepository
{
    public interface IReadDbRepository<T> where T : class, IEntity
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync(Guid Id);
        #region GetAsync
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        #endregion
        #region ToListAsync
        Task<IList<T>> ToListAsync();
        Task<IList<T>> ToListAsync(Expression<Func<T, bool>> expression);
        Task<IList<T>> ToListAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        Task<IList<T>> ToListAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes);
        #endregion
        #region GetCountAsync
        Task<int> GetCountAsync();
        Task<int> GetCountAsync(Expression<Func<T, bool>> expression);
        #endregion
        IQueryable<T> Query();
    }
}
