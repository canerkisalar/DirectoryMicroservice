using Phonebook.Core.Repositories.EntityFramework;
using Phonebook.Services.Report.Infrastructure.Abstract.Report;
using Phonebook.Services.Report.Models.Report;

namespace Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework.Report
{
    public class EfReportItemDal : EfEntityRepositoryBase<ReportItem, Context>, IReportItemDal
    {
        public EfReportItemDal(Context context) : base(context)
        {
        }
    }
}