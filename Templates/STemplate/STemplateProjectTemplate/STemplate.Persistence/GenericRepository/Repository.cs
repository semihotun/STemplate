using Microsoft.EntityFrameworkCore;
using STemplate.Domain.SeedWork;
using STemplate.Persistence.Context;
using System.Linq.Expressions;
namespace STemplate.Persistence.GenericRepository
{
    public class Repository<TEntity>(ICoreDbContext context) : IRepository<TEntity>
             where TEntity : class, IEntity
    {
        private readonly ICoreDbContext Context = context;
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
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var data = await Context.Set<TEntity>().AddAsync(entity);
            return data.Entity;
        }
        public void RemoveRange(List<TEntity> entity)
        {
            Context.Set<TEntity>().RemoveRange(entity);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().AnyAsync(expression);
        }
        public async Task<TEntity?> GetByIdAsync(Guid Id)
        {
            return await Context.Set<TEntity>().FindAsync(Id);
        }
        #region Get 
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            foreach (var item in includes)
            {
                query.Include(item);
            }
            return await query.FirstOrDefaultAsync(expression);
        }
        #endregion
        #region ToListAsync
        public async Task<IList<TEntity>> ToListAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
        public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().Where(expression).ToListAsync();
        }
        public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.Where(expression).ToListAsync();
        }
        public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();
        }
        #endregion
        #region GetCountAsync
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().CountAsync(expression);
        }
        public async Task<int> GetCountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }
        #endregion
        public IQueryable<TEntity> Query() => Context.Set<TEntity>();
    }
}
