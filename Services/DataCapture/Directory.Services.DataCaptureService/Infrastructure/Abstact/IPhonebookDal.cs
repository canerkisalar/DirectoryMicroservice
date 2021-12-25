using Phonebook.Core.Repositories;

namespace Phonebook.Services.DataCapture.Infrastructure.Abstact
{
    public interface IPhonebookDal : IEntityRepository<Domain.Phonebook>
    {
    }
}
