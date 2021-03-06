using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Phonebook.Core.Domain;

namespace Phonebook.Core.Repositories
{
    public interface IEntityRepository<TEntity> where TEntity : class, IEntity,new()
    {
        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Delete(Guid id);

        Task DeleteAll(Expression<Func<TEntity, bool>> filter=null);

        Task<TEntity> FindAndUpdate(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update);

        Task<int> UpdateOneAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update);

    }
}
