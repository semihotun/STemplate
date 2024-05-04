using STemplate.Domain.SeedWork;
using System.Linq.Expressions;
namespace STemplate.Persistence.GenericRepository
{
    public interface IWriteDbRepository<T> where T : class, IEntity
    {
        Task<T> AddAsync(T entity);
        void AddRange(List<T> entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        T Update(T entity);
        void Remove(T entity);
        void RemoveRange(List<T> entity);
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
