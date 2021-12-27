

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Phonebook.Core.Domain;

namespace Phonebook.Core.Repositories.MongoDb
{
    public class MongoEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, IMongoBaseEntity, new()
    {
        protected readonly IMongoCollection<TEntity> Collection;

        public MongoEntityRepositoryBase(MongoDbSettings options)
        {
            var client = new MongoClient(options.ConnectionString);
            var db = client.GetDatabase(options.Database);
            this.Collection = db.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? await Collection.Find(x => true).ToListAsync()
                : await Collection.Find(filter).ToListAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return filter == null
                ? await Collection.Find(x => true).FirstOrDefaultAsync()
                : await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await Collection.InsertOneAsync(entity, options);
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> Delete(Guid id)
        {
            var result = await Collection.FindOneAndDeleteAsync(x => x.Id == id);
            return result;
        }

        public async Task DeleteAll(Expression<Func<TEntity, bool>> filter = null)
        {
            await Collection.DeleteManyAsync(c => true);
        }

        public async Task<TEntity> FindAndUpdate(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            var result = await Collection.FindOneAndUpdateAsync(filter, update);
           return result;
        }

        public async Task<int> UpdateOneAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            var result = await Collection.UpdateOneAsync(filter, update);
            
            return (int) result.ModifiedCount;
        }
    }
}
