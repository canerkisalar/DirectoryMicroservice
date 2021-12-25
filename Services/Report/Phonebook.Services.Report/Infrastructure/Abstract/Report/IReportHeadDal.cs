using System;
using System.Threading.Tasks;
using Phonebook.Core.Repositories;
using Phonebook.Services.Report.Enums;
using Phonebook.Services.Report.Models.Report;

namespace Phonebook.Services.Report.Infrastructure.Abstract.Report
{
    public interface IReportHeadDal : IEntityRepository<ReportHead>
    {
        Task<ReportHead> UpdateReportStatusById(Guid reportId, ReportStatusTypes status);
    }
}
