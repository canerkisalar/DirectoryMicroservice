using Phonebook.Core.Repositories.EntityFramework;
using Phonebook.Services.Report.Infrastructure.Abstract;
using Phonebook.Services.Report.Models;

namespace Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework
{
    public class EfContactDal : EfEntityRepositoryBase<Contact,Context>,IContactDal
    {
        public EfContactDal(Context context) : base(context)
        {
        }
    }
}
