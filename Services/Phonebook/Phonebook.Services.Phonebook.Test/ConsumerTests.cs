using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using NUnit.Framework;
using Phonebook.Core.Messages.Report;
using Phonebook.Core.Repositories.MongoDb;
using Phonebook.Services.Phonebook.Consumers;
using Phonebook.Services.Phonebook.Infrastructure.Concrete.MongoDb;
using Phonebook.Services.Phonebook.Services.Concrete;
using Phonebook.Services.Phonebook.Test.Mock;

namespace Phonebook.Services.Phonebook.Test
{
    public class ConsumerTests
    {
        private static MongoPhonebookDal _phonebookDal;

        [SetUp]
        public void Setup()
        {
            _phonebookDal = new MongoPhonebookDal(new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                Database = "PhonebookTestDb"
            });
            _phonebookDal.DeleteAll();
        }

        private static GiveAllPhonebooksMessageCommandConsumer GetHandler()
        {
            var phonebookService = new PhonebookService(AutoMapperConfiguration.SetMapper(),
                SendEndpointFactory.GetSendEndpoint().Object, _phonebookDal);

            var consumer = new GiveAllPhonebooksMessageCommandConsumer(phonebookService);

            return consumer;

        }

        [Test]
        public async Task t010_GivePhonebooks()
        {

            var message = new GetAllPhonebooksMessageCommand();
            message.ReportId = Guid.NewGuid();
            var handler = GetHandler();
            var context = Moq.Mock.Of<ConsumeContext<GetAllPhonebooksMessageCommand>>(_ => _.Message == message);
            var result = handler.Consume(context);

        }
    }
}
