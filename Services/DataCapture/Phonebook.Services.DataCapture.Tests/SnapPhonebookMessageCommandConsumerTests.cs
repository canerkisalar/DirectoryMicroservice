using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Moq;
using NUnit.Framework;
using Phonebook.Core.Domain.Enums;
using Phonebook.Core.Messages.DataCapture;
using Phonebook.Services.DataCapture.Consumers;
using Phonebook.Services.DataCapture.Infrastructure.Abstact;
using Phonebook.Services.DataCapture.Infrastructure.Concrete.EntityFramework;
using Phonebook.Services.DataCapture.Tests.Mock;
using Contact = Phonebook.Core.Messages.DataCapture.Contact;

namespace Phonebook.Services.DataCapture.Tests
{
    public class Tests
    {
        private static Context          _db;
        private static Domain.Phonebook _phonebook;


        [SetUp]
        public void Setup()
        {
            _db = DatabaseContextFactory.GetInstance;
            GenerateData();
        }

        public static void GenerateData()
        {
            _phonebook = new Domain.Phonebook()
                         {
                             PhonebookId = Guid.NewGuid(),
                             Company     = "TestCompany",
                             Name        = "TestName",
                             Surname     = "TestSurName",
                             Contacts    = new List<Domain.Contact>() { new() { ContactId = Guid.NewGuid(), ContactInformation = "12345648798", ContactType = ContactTypes.Phone.ToString() }, }
                         };

            _db.Phonebooks.Add(_phonebook);
            _db.SaveChanges();
        }

        private static SnapPhonebookMessageCommandConsumer GetHandler(Context db)
        {
            var phoneBookDal     = new EfPhonebookDal(db);
            var phoneBookDalMock = new Mock<IPhonebookDal>();
            phoneBookDalMock.Setup(s => s.Add(It.IsAny<Domain.Phonebook>())).ReturnsAsync(new Domain.Phonebook());
            var consumer = new SnapPhonebookMessageCommandConsumer(phoneBookDal);
            return consumer;
        }


        [Test]
        public async Task t010_TakeMessageAndSaveTest()
        {
            var message = new SnapPhonebookMessageCommand()
                          {
                              Company = "TestCompany",
                              Contacts = new List<Contact>()
                                         {
                                             new() { Id = Guid.NewGuid(), ContactInformation = "12345648798", ContactType     = ContactTypes.Phone.ToString() },
                                             new() { Id = Guid.NewGuid(), ContactInformation = "Bilecik", ContactType         = ContactTypes.Location.ToString() },
                                             new() { Id = Guid.NewGuid(), ContactInformation = "gmail@gmail.com", ContactType = ContactTypes.Email.ToString() }
                                         },
                              Modification = (int)ModiTypes.Create,
                              Id           = Guid.NewGuid(),
                              Name         = "TestName",
                              Surname      = "TestSurname"
                          };
            var handler = GetHandler(_db);
            var context = Moq.Mock.Of<ConsumeContext<SnapPhonebookMessageCommand>>(_ => _.Message == message);
            var result  = handler.Consume(context);
            result.Exception.Should().BeNull();
            var phoneBook = _db.Phonebooks.FirstOrDefault(p => p.PhonebookId == message.Id);
            phoneBook.Should().NotBeNull();
            var locationContact = phoneBook.Contacts.First(c => c.ContactType == ContactTypes.Location.ToString());
            locationContact.Should().NotBeNull();
            locationContact.ContactInformation.Should().Be(message.Contacts.First(c => c.ContactType == ContactTypes.Location.ToString()).ContactInformation);
        }

        [Test]
        public async Task t020_TakeMessageAndUpdateTest()
        {
            var message = new SnapPhonebookMessageCommand()
                          {
                              Company = "TestUpdatedCompany",
                              Contacts = new List<Contact>()
                                         {
                                             new() { Id = Guid.NewGuid(), ContactInformation = "12345648798", ContactType     = ContactTypes.Phone.ToString() },
                                             new() { Id = Guid.NewGuid(), ContactInformation = "Bilecik", ContactType         = ContactTypes.Location.ToString() },
                                             new() { Id = Guid.NewGuid(), ContactInformation = "gmail@gmail.com", ContactType = ContactTypes.Email.ToString() }
                                         },
                              Modification = (int)ModiTypes.Update,
                              Id           = _phonebook.PhonebookId,
                              Name         = "TestUpdatedName",
                              Surname      = "TestUpdatedSurname"
                          };
            var handler = GetHandler(_db);
            var context = Moq.Mock.Of<ConsumeContext<SnapPhonebookMessageCommand>>(_ => _.Message == message);
            var result  = handler.Consume(context);
            result.Exception.Should().BeNull();
            var phoneBook = _db.Phonebooks.First(p => p.PhonebookId == message.Id);
            phoneBook.Should().NotBeNull();
            var locationContact = phoneBook.Contacts.First(c => c.ContactType == ContactTypes.Location.ToString());
            var phoneContact    = phoneBook.Contacts.First(c => c.ContactType == ContactTypes.Phone.ToString());
            var emailContact    = phoneBook.Contacts.First(c => c.ContactType == ContactTypes.Email.ToString());
            locationContact.Should().NotBeNull();
            phoneContact.Should().NotBeNull();
            emailContact.Should().NotBeNull();

            phoneBook.PhonebookId.Should().Be(_phonebook.PhonebookId);
            phoneBook.Name.Should().Be(message.Name);
        }

        [Test]
        public async Task t030_TakeMessageAndDeleteTest()
        {
            var message = new SnapPhonebookMessageCommand()
                          {
                              Modification = (int)ModiTypes.Delete,
                              Id           = _phonebook.PhonebookId,
                          };
            var handler = GetHandler(_db);
            var context = Moq.Mock.Of<ConsumeContext<SnapPhonebookMessageCommand>>(_ => _.Message == message);
            var result  = handler.Consume(context);
            result.Exception.Should().BeNull();
            var phoneBook = _db.Phonebooks.FirstOrDefault(p => p.PhonebookId == message.Id);
            phoneBook.Should().BeNull();

        }
    }
}