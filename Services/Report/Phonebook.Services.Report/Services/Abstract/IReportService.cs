
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phonebook.Core.Domain.Dtos;
using Phonebook.Core.Messages;
using Phonebook.Core.Messages.Report;
using Phonebook.Services.Report.Dtos;
using Phonebook.Services.Report.Dtos.Reports;
using Phonebook.Services.Report.Enums;
using Phonebook.Services.Report.Models.Report;

namespace Phonebook.Services.Report.Services.Abstract
{
    public interface IReportService
    {
        Task<Response<ReportHead>> GetInstantDataAndReportAsync();

        Task<Core.Domain.Dtos.Response<List<ReportHeadDto>>> GetLocationReportList();

        Task<Response<List<LocationReportDto>>> GetLocationReportByIdAsync(Guid reportId);

        Task<Response<GetAllPhonebooksMessageCommand>> GetBatchData(Guid reportId);

        Task PrepLocationReport(AllPhonebooksMessageCommand context, SourceTypes sourceType, List<Models.Phonebook> dataFromDb = null);
    }
}
