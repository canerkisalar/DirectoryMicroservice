using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MassTransit;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Phonebook.Core.Domain.Enums;
using Phonebook.Core.Messages.Report;
using Phonebook.Services.Report.Dtos.Reports;
using Phonebook.Services.Report.Enums;
using Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework;
using Phonebook.Services.Report.Infrastructure.Concrete.EntityFramework.Report;
using Phonebook.Services.Report.Models;
using Phonebook.Services.Report.Models.Report;
using Phonebook.Services.Report.Services.Concrete;
using Phonebook.Services.Report.Tests.Mock;

namespace Phonebook.Services.Report.Tests
{
    public class ReportServiceTests
    {
        private static Context _db;
        private static ReportHead _reportHead;
        private static ReportItem _reportItem;

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


            var locationItems = new List<LocationReportDto>();
            locationItems.Add(new LocationReportDto()
            {
                CountPerson = 1,
                CountPhoneNumbers = 1,
                Location = "Bilecik"
            });
            locationItems.Add(new LocationReportDto()
            {
                CountPerson = 3,
                CountPhoneNumbers = 2,
                Location = "Eskişehir"
            });

            _reportItem = new ReportItem()
            {
                ReportHeadId = _reportHead.ReportHeadId,
                ReportItemId = Guid.NewGuid(),
                Code = JsonConvert.SerializeObject(locationItems)
            };


            _db.ReportItem.Add(_reportItem);
            _db.SaveChanges();


        }

        private static ReportService GetHandler(Context db)
        {
           
            EndpointConvention.Map<TestMessage>(new Uri("http://random"));

            var reportService = new ReportService( new EfReportHeadDal(db), new EfReportItemDal(db), SendEndpointFactory.GetSendEndpoint().Object, AutoMapperConfiguration.SetMapper());

            return reportService;
        }

        [Test]
        public async Task t010_GetLocationReportById()
        {
            var handler = GetHandler(_db);
            var result = await handler.GetLocationReportByIdAsync(_reportItem.ReportHeadId);
            
        }

        [Test]
        public async Task t020_GetLocationReportList()
        {
            var handler = GetHandler(_db);
            var result = await handler.GetLocationReportList();
            result.Data.Should().NotBeEmpty();
        }

        [Test]
        public async Task t030_PrepReport()
        {
            var handler = GetHandler(_db);

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
                new() { ContactId = Guid.NewGuid(), ContactInformation = "Eskişehir", ContactType         = ContactTypes.Location.ToString() },
                new() { ContactId = Guid.NewGuid(), ContactInformation = "gmail@gmail.com", ContactType = ContactTypes.Email.ToString() }
            }
            });

            var sourceType = SourceTypes.FromMessageQueue;
            var data = new AllPhonebooksMessageCommand()
            {
                ReportId = _reportHead.ReportHeadId,
                PhonebookAllData = JsonConvert.SerializeObject(phonebooks)
            };

            await handler.PrepLocationReport(sourceType, data);
            var reportHead = _db.ReportHead.ToList().Where(x => x.ReportHeadId == _reportHead.ReportHeadId);
            reportHead.Should().NotBeEmpty();
            reportHead.Select(x => x.Status).FirstOrDefault().Should().Be(ReportStatusTypes.Done.ToString());
        }

        [Test]
        public async Task t020_GetDataAndCreateReport()
        {
            var handler = GetHandler(_db);
            var result = await handler.GetInstantDataAndReportAsync();

            var requestedReport = _db.ReportHead.FirstOrDefault(r => r.ReportHeadId == result.Data.ReportHeadId);

            requestedReport.Should().NotBeNull();
            requestedReport.Status.Should().Be(ReportStatusTypes.Preparing.ToString());
        }


    }
}
