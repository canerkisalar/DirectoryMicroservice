

using System.Threading.Tasks;
using MassTransit;
using Phonebook.Core.Messages.Report;
using Phonebook.Services.Report.Enums;
using Phonebook.Services.Report.Infrastructure.Abstract.Report;
using Phonebook.Services.Report.Services.Abstract;

namespace Phonebook.Services.Report.Consumers
{
    public class GetAllPhonebooksMessageCommandConsumer : IConsumer<AllPhonebooksMessageCommand>
    {
        private readonly IReportHeadDal _reportHeadDal;
        private readonly IReportItemDal _reportItemDal;
        private readonly IReportService _reportService;

        public GetAllPhonebooksMessageCommandConsumer(IReportHeadDal reportHeadDal, IReportItemDal reportItemDal, IReportService reportService)
        {
            _reportHeadDal = reportHeadDal;
            _reportItemDal = reportItemDal;
            _reportService = reportService;
        }

        

        public async Task Consume(ConsumeContext<AllPhonebooksMessageCommand> context)
        {
            if (context.Message.PhonebookAllData != null)
            {
                await _reportService.PrepLocationReport(SourceTypes.FromMessageQueue, context.Message);
            }
            //if (allData != null)
            //{
            //    await using var ctx = new Context();
            //    ctx.Contacts.FromSqlRaw("TRUNCATE Contacts");
            //    ctx.Phonebooks.FromSqlRaw("TRUNCATE Phonebooks");
            //    ctx.BulkInsert(allData, opt =>
            //         opt.IncludeGraph = true
            //    );
            //    ctx.Phonebooks.AddRange(allData);
            //    await ctx.SaveChangesAsync();
            //}

        }

       
    }
}

