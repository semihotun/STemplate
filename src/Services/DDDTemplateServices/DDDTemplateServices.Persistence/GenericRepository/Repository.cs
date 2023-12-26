﻿using DDDTemplateService.Domain.SeedWork;
using DDDTemplateServices.Persistence.Context;
using DDDTemplateServices.Persistence.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace DDDTemplateServices.Persistence.GenericRepository
{
    public class Repository<TEntity> : IRepository<TEntity>
          where TEntity : class, IEntity
    {
        protected readonly CoreDbContext Context;
        private readonly IMediator mediator;
        public Repository(CoreDbContext context, IMediator mediator)
        {
            Context = context;
            this.mediator = mediator;
        }
        public TEntity? GetById(int Id)
        {
            return Context.Set<TEntity>().Find(Id);
        }
        public TEntity Add(TEntity entity)
        {
            return Context.Add(entity).Entity;
        }
        public void AddRange(List<TEntity> entity)
        {
            Context.AddRange(entity);
        }
        public TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            return entity;
        }
        public void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }
        public void DeleteRange(List<TEntity> entity)
        {
            Context.RemoveRange(entity);
        }
        public TEntity? Get(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().FirstOrDefault(expression);
        }
        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(expression);
        }
        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>>? expression = null)
        {
            return expression == null ? Context.Set<TEntity>().AsNoTracking() : Context.Set<TEntity>().Where(expression).AsNoTracking();
        }
        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? expression = null)
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
#nullable disable
        public async Task<TResult> BeginTransaction<TResult>(Func<Task<TResult>> action, Action successAction = null, Action<Exception> exceptionAction = null)
        {
            var result = default(TResult);
            var strategy = Context.Database.CreateExecutionStrategy();
            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    using (var tx = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            result = await action();
                            await mediator.DispatchDomainEventsAsync(Context);
                            await Context.SaveChangesAsync();
                            tx.Commit();
                        }
                        catch (Exception)
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                    successAction?.Invoke();
                });
            }
            catch (Exception ex)
            {
                if (exceptionAction == null)
                    throw;
                else
                    exceptionAction(ex);
            }
            return result;
        }
    }
}