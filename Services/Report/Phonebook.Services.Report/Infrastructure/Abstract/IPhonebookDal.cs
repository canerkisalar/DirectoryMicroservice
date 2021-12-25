using Phonebook.Core.Repositories;

namespace Phonebook.Services.Report.Infrastructure.Abstract
{
    public interface IPhonebookDal : IEntityRepository<Models.Phonebook>
    {
    }
}
