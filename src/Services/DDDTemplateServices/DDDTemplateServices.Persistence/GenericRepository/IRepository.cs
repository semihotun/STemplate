using DDDTemplateService.Domain.SeedWork;
using System.Linq.Expressions;
namespace DDDTemplateServices.Persistence.GenericRepository
{
    public interface IRepository<T> where T : class, IEntity
    {
        T Add(T entity);
        void AddRange(List<T> entity);
        T Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        IEnumerable<T> GetList(Expression<Func<T, bool>>? expression = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? expression = null);
        T? Get(Expression<Func<T, bool>> expression);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> Query();
        Task<int> GetCountAsync(Expression<Func<T, bool>>? expression = null);
        int GetCount(Expression<Func<T, bool>>? expression = null);
        T? GetById(int Id);
        Task<TResult> BeginTransaction<TResult>(Func<Task<TResult>> action, Action? successAction = null, Action<Exception>? exceptionAction = null);
    }
}
