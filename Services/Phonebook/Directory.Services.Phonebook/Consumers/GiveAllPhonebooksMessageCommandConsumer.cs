using System.Threading.Tasks;
using MassTransit;
using Newtonsoft.Json;
using Phonebook.Core.Messages.Report;
using Phonebook.Services.Phonebook.Services.Abstract;

namespace Phonebook.Services.Phonebook.Consumers
{
    public class GiveAllPhonebooksMessageCommandConsumer : IConsumer<GetAllPhonebooksMessageCommand>
    {
        private readonly IPhonebookService _phonebookService;

        public GiveAllPhonebooksMessageCommandConsumer(IPhonebookService phonebookService)
        {
            _phonebookService = phonebookService;
        }


        public async Task Consume(ConsumeContext<GetAllPhonebooksMessageCommand> context)
        {
            var allList =  _phonebookService.GetAllWithDetailAsync().Result.Data;

            var toJson = JsonConvert.SerializeObject(allList);

            await _phonebookService.SendDataToReportService(toJson,context.Message.ReportId);

        }
    }
}
