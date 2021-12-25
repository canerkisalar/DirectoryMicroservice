using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.Core.Domain;

namespace Phonebook.Core.Repositories.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            await using var context = new TContext();
            return filter == null
                ? context.Set<TEntity>().ToList()
                : context.Set<TEntity>().Where(filter).ToList();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            await using var context = new TContext();
            return context.Set<TEntity>().SingleOrDefault(filter);
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await using var context = new TContext();
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
             await using var context = new TContext();
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(Guid id)
        {
            await using var context = new TContext();
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }
    }
}
