
using Phonebook.Core.Repositories;

namespace Phonebook.Services.Phonebook.Infrastructure.Abstract
{
    public interface IPhonebookDal : IEntityRepository<Models.Phonebook>
    {
    }
}
