using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDTemplateServices.Persistence.Context
{
    public interface ICoreDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
        Task<TResult> BeginTransaction<TResult>(Func<Task<TResult>> action, Action? successAction = null, Action<Exception>? exceptionAction = null);
    }
}
