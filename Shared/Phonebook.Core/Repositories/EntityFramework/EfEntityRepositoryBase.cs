using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Phonebook.Core.Domain;

namespace Phonebook.Core.Repositories.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>, IDisposable
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        private TContext _context;

        protected EfEntityRepositoryBase(TContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().ToList()
                : _context.Set<TEntity>().Where(filter).ToList();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().SingleOrDefault(filter);
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await using var context = new TContext();
            _context.Set<TEntity>().Add(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(Guid id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAll(Expression<Func<TEntity, bool>> filter=null)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> FindAndUpdate(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateOneAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            throw new NotImplementedException();
        }
    }
}