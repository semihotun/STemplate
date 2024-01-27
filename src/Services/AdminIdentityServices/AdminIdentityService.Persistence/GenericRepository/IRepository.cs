using AdminIdentityService.Domain.SeedWork;
using System.Linq.Expressions;
namespace AdminIdentityService.Persistence.GenericRepository
{
    public interface IRepository<T> where T : class, IEntity
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void AddRange(List<T> entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        T Update(T entity);
        void Remove(T entity);
        void RemoveRange(List<T> entity);
        T? GetById(Guid Id);
        Task<T?> GetByIdAsync(Guid Id);
        IEnumerable<T> GetEnumerable(Expression<Func<T, bool>>? expression = null);
        Task<IList<T>> ToListAsync(Expression<Func<T, bool>>? expression = null);
        T? Get(Expression<Func<T, bool>> expression);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        int GetCount(Expression<Func<T, bool>>? expression = null);
        Task<int> GetCountAsync(Expression<Func<T, bool>>? expression = null);
        bool Any(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> Query();
    }
}
