using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Phonebook.Core.Domain.Enums;
using Phonebook.Core.Messages.Report;
using Phonebook.Services.Report.Consumers;
using Phonebook.Services.Report.Enums;
using Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework;
using Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework.Report;
using Phonebook.Services.Report.Models;
using Phonebook.Services.Report.Models.Report;
using Phonebook.Services.Report.Services.Concrete;
using Phonebook.Services.Report.Tests.Mock;
using Phonebook = Phonebook.Services.Report.Models.Phonebook;

namespace Phonebook.Services.Report.Tests
{
    public class Tests
    {
        private static Context _db;
        private static ReportHead _reportHead;

        [SetUp]
        public void Setup()
        {
            _db = DatabaseContextFactory.GetInstance;
            GenerateData();

        }

        public static void GenerateData()
        {
            _reportHead = new ReportHead()
            {
               ReportHeadId = Guid.NewGuid(),
               Status = ReportStatusTypes.Preparing.ToString(),
               PreparationDate = DateTime.MinValue,
               RequestDate = DateTime.Now
            };
          
            _db.ReportHead.Add(_reportHead);
            _db.SaveChanges();
        }

        private static GetAllPhonebooksMessageCommandConsumer GetHandler(Context db)
        {
            var reportService = new ReportService(new EfPhonebookDal(db),new EfReportHeadDal(db),new EfReportItemDal(db),null,null);

            var consumer = new GetAllPhonebooksMessageCommandConsumer(new EfReportHeadDal(db),new EfReportItemDal(db),reportService);

            return consumer;

        }



        [Test]
        public async Task t010_TakePhonebooksAndCreateReport()
        {
            var phonebooks = new List<Models.Phonebook>();

            phonebooks.Add(new Models.Phonebook()
            {
                Name = "Test Name",
                Surname = "Test Surname",
                Company = "Test Company",
                PhonebookId = Guid.NewGuid(),
                Contacts = new List<Contact>()
                {
                    new() { ContactId = Guid.NewGuid(), ContactInformation = "12345648798", ContactType     = ContactTypes.Phone.ToString() },
                    new() { ContactId = Guid.NewGuid(), ContactInformation = "Bilecik", ContactType         = ContactTypes.Location.ToString() },
                    new() { ContactId = Guid.NewGuid(), ContactInformation = "gmail@gmail.com", ContactType = ContactTypes.Email.ToString() }
                }
            });

            phonebooks.Add(new Models.Phonebook()
            {
                Name = "Test 2 Name",
                Surname = "Test 2 Surname",
                Company = "Test 2 Company",
                PhonebookId = Guid.NewGuid(),
                Contacts = new List<Contact>()
                {
                    new() { ContactId = Guid.NewGuid(), ContactInformation = "12345648798", ContactType     = ContactTypes.Phone.ToString() },
                    new() { ContactId = Guid.NewGuid(), ContactInformation = "Eskiþehir", ContactType         = ContactTypes.Location.ToString() },
                    new() { ContactId = Guid.NewGuid(), ContactInformation = "gmail@gmail.com", ContactType = ContactTypes.Email.ToString() }
                }
            });

            var message = new AllPhonebooksMessageCommand()
            {
                ReportId = _reportHead.ReportHeadId,
                PhonebookAllData = JsonConvert.SerializeObject(phonebooks)
            };
            var handler = GetHandler(_db);
            var context = Moq.Mock.Of<ConsumeContext<AllPhonebooksMessageCommand>>(_ => _.Message == message);
            var result = handler.Consume(context);
            var reportHead =   _db.ReportHead.ToList().Where(x=>x.ReportHeadId==_reportHead.ReportHeadId);
            reportHead.Should().NotBeEmpty();
            reportHead.Select(x => x.Status).FirstOrDefault().Should().Be(ReportStatusTypes.Done.ToString());


      
        }
    }
}