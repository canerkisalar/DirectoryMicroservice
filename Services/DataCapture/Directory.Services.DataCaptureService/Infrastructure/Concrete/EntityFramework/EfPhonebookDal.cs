using Phonebook.Core.Repositories.EntityFramework;
using Phonebook.Services.DataCapture.Infrastructure.Abstact;

namespace Phonebook.Services.DataCapture.Infrastructure.Concrete.EntityFramework
{
    public class EfPhonebookDal : EfEntityRepositoryBase<Domain.Phonebook,Context>,IPhonebookDal
    {
    }
}
