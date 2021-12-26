
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Phonebook.Core.Repositories.MongoDb;
using Phonebook.Services.Phonebook.Infrastructure.Abstract;

namespace Phonebook.Services.Phonebook.Infrastructure.Concrete.MongoDb
{
    public class MongoPhonebookDal : MongoEntityRepositoryBase<Models.Phonebook>, IPhonebookDal
    {
       
        public MongoPhonebookDal(Core.Repositories.MongoDb.MongoDbSettings options) : base(options)
        {
    
        }

       
    }
}
