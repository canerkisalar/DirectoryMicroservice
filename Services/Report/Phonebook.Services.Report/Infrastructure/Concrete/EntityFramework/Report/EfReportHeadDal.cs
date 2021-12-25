
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.Core.Repositories.EntityFramework;
using Phonebook.Services.Report.Enums;
using Phonebook.Services.Report.Models.Report;
using Phonebook.Services.Report.Infrastructure.Abstract.Report;

namespace Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework.Report
{
    public class EfReportHeadDal : EfEntityRepositoryBase<ReportHead,Context>,IReportHeadDal
    {
        public async Task<ReportHead> UpdateReportStatusById(Guid reportId, ReportStatusTypes status)
        {
            using (var context = new Context())
            {
               
                var entity = await context.Set<ReportHead>().FindAsync(reportId);
                entity.Status = status.ToString();
                entity.PreparationDate = DateTime.MinValue;
                if (status == ReportStatusTypes.Done)
                {
                    entity.PreparationDate = DateTime.Now;
                }
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return entity;
            }
        }
    }
}
