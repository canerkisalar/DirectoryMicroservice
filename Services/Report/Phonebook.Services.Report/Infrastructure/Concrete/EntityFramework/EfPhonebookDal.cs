using Phonebook.Core.Repositories.EntityFramework;
using Phonebook.Services.Report.Infrastructure.Abstract;

namespace Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework
{
    public class EfPhonebookDal : EfEntityRepositoryBase<Models.Phonebook,Context>,IPhonebookDal
    {
        public EfPhonebookDal(Context context) : base(context)
        {
        }
       
    }
}
