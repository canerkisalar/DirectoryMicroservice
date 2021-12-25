using Phonebook.Core.Repositories.EntityFramework;
using Phonebook.Services.DataCapture.Domain;
using Phonebook.Services.DataCapture.Infrastructure.Abstact;

namespace Phonebook.Services.DataCapture.Infrastructure.Concrete.EntityFramework
{
    public class EfContactDal : EfEntityRepositoryBase<Contact,Context>,IContactDal

    {
    }
}
