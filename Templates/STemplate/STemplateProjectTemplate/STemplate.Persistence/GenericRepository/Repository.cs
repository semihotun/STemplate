using Microsoft.EntityFrameworkCore;
using STemplate.Domain.SeedWork;
using STemplate.Persistence.Context;
using System.Linq.Expressions;
namespace STemplate.Persistence.GenericRepository
{
    public class Repository<TEntity>(ICoreDbContext context) : IRepository<TEntity>
          where TEntity : class, IEntity
    {
        protected readonly ICoreDbContext Context = context;

        public TEntity? GetById(Guid Id)
        {
            return Context.Set<TEntity>().Find(Id);
        }
        public async Task<TEntity?> GetByIdAsync(Guid Id)
        {
            return await Context.Set<TEntity>().FindAsync(Id);
        }
        public TEntity Add(TEntity entity)
        {
            var data = Context.Set<TEntity>().Add(entity);
            return data.Entity;
        }
        public void AddRange(List<TEntity> entity)
        {
            Context.Set<TEntity>().AddRange(entity);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            await Context.Set<TEntity>().AddRangeAsync(entity);
        }
        public TEntity Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return entity;
        }
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
        public void RemoveRange(List<TEntity> entity)
        {
            Context.Set<TEntity>().RemoveRange(entity);
        }
        public TEntity? Get(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().FirstOrDefault(expression);
        }
        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().Any(expression);
        }
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }
        public IEnumerable<TEntity> GetEnumerable(Expression<Func<TEntity, bool>>? expression = null)
        {
            return expression == null ? Context.Set<TEntity>().AsNoTracking() : Context.Set<TEntity>().Where(expression).AsNoTracking();
        }
        public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? expression = null)
        {
            return expression == null ? await Context.Set<TEntity>().ToListAsync() :
                 await Context.Set<TEntity>().Where(expression).ToListAsync();
        }
        public IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression == null)
                return await Context.Set<TEntity>().CountAsync();
            else
                return await Context.Set<TEntity>().CountAsync(expression);
        }
        public int GetCount(Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression == null)
                return Context.Set<TEntity>().Count();
            else
                return Context.Set<TEntity>().Count(expression);
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var data = await Context.Set<TEntity>().AddAsync(entity);
            return data.Entity;
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().AnyAsync(expression);
        }
    }
}
