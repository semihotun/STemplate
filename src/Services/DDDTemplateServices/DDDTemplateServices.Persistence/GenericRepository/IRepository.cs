using DDDTemplateService.Domain.SeedWork;
using System.Linq.Expressions;
namespace DDDTemplateServices.Persistence.GenericRepository
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
        T? GetById(int Id);
        IEnumerable<T> GetList(Expression<Func<T, bool>>? expression = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? expression = null);
        T? Get(Expression<Func<T, bool>> expression);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        int GetCount(Expression<Func<T, bool>>? expression = null);
        Task<int> GetCountAsync(Expression<Func<T, bool>>? expression = null);
        bool Any(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> Query();
    }
}
