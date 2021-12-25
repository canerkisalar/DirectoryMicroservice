using Phonebook.Core.Repositories;
using Phonebook.Services.DataCapture.Domain;

namespace Phonebook.Services.DataCapture.Infrastructure.Abstact
{
    public interface IContactDal: IEntityRepository<Contact>
    {
    }
}
