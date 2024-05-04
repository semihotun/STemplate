using Microsoft.EntityFrameworkCore;
using STemplate.Domain.SeedWork;
using STemplate.Persistence.Context;
using System.Linq.Expressions;
namespace STemplate.Persistence.GenericRepository
{
    public class WriteDbRepository<TEntity> : IWriteDbRepository<TEntity>
    where TEntity : BaseEntity, IEntity
    {
        private readonly ICoreDbContext _writeContext;
        public WriteDbRepository(ICoreDbContext writeContext)
        {
            _writeContext = writeContext;
        }
        public void AddRange(List<TEntity> entity)
        {
            _writeContext.Set<TEntity>().AddRange(entity);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            await _writeContext.Set<TEntity>().AddRangeAsync(entity);
        }
        public TEntity Update(TEntity entity)
        {
            _writeContext.Set<TEntity>().Update(entity);
            return entity;
        }
        public void Remove(TEntity entity)
        {
            entity.Deleted = true;
            _writeContext.Set<TEntity>().Update(entity);
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return (await _writeContext.Set<TEntity>().AddAsync(entity)).Entity;
        }
        public void RemoveRange(List<TEntity> entity)
        {
            foreach (var item in entity)
            {
                Remove(item);
            }
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _writeContext.Query<TEntity>().AnyAsync(expression);
        }
        public async Task<TEntity?> GetByIdAsync(Guid Id)
        {
            var entity = await _writeContext.Set<TEntity>().FindAsync(Id);
            if (entity?.Deleted == false)
            {
                return entity;
            }
            return null;
        }
        #region Get 
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _writeContext.Query<TEntity>().FirstOrDefaultAsync(expression);
        }
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _writeContext.Query<TEntity>();
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
            return await _writeContext.Query<TEntity>().ToListAsync();
        }
        public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _writeContext.Query<TEntity>().Where(expression).ToListAsync();
        }
        public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _writeContext.Query<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.Where(expression).ToListAsync();
        }
        public async Task<IList<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _writeContext.Query<TEntity>();
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
            return await _writeContext.Query<TEntity>().CountAsync(expression);
        }
        public async Task<int> GetCountAsync()
        {
            return await _writeContext.Query<TEntity>().CountAsync();
        }
        #endregion
        public IQueryable<TEntity> Query() => _writeContext.Query<TEntity>();
    }
}
