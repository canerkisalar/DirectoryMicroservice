using Phonebook.Core.Repositories;
using Phonebook.Services.Report.Models;

namespace Phonebook.Services.Report.Infrastructure.Abstract
{
    public interface IContactDal: IEntityRepository<Contact>
    {
    }
}
