
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Phonebook.Core.Domain.Enums;
using Phonebook.Core.Repositories;
using Phonebook.Core.Repositories.MongoDb;
using Phonebook.Services.Phonebook.Dtos.Contact;
using Phonebook.Services.Phonebook.Dtos.Phonebook;
using Phonebook.Services.Phonebook.Infrastructure.Concrete.MongoDb;
using Phonebook.Services.Phonebook.Models;
using Phonebook.Services.Phonebook.Services.Concrete;
using Phonebook.Services.Phonebook.Test.Mock;


namespace Phonebook.Services.Phonebook.Test
{
    public class Tests
    {
        private static MongoPhonebookDal _phonebookDal;


        private static Models.Phonebook PhonebookCreator()
        {
            var phonebookCreate = new Models.Phonebook()
            {
                Id = Guid.NewGuid(),
                Company = "TestCompany - TempCreate",
                Surname = "TestSurname - TempCreate",
                Name = "TestName - TempCreate",
                Contacts = new List<Contact>()
                {
                    new Contact()
                    {
                        Id = Guid.NewGuid(),
                        ContactType = ContactTypes.Phone.ToString(),
                        ContactInformation = "1234567890"
                    },
                    new Contact()
                    {
                        Id = Guid.NewGuid(),
                        ContactType = ContactTypes.Location.ToString(),
                        ContactInformation = "Bilecik"
                    },
                    new Contact()
                    {
                        Id = Guid.NewGuid(),
                        ContactType = ContactTypes.Email.ToString(),
                        ContactInformation = "caner@kisalar.net"
                    }
                }
            };
            return phonebookCreate;
        }

        private static List<Models.Phonebook> PhonebookCreatorMany()
        {
            var returnList = new List<Models.Phonebook>();

            returnList.AddRange(new List<Models.Phonebook>()
            {
                new Models.Phonebook()
                {
                    Id = Guid.NewGuid(),
                    Company = "TestCompany - TempCreate",
                    Surname = "TestSurname - TempCreate",
                    Name = "TestName - TempCreate",
                    Contacts = new List<Contact>()
                    {
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Phone.ToString(),
                            ContactInformation = "1234567890"
                        },
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Location.ToString(),
                            ContactInformation = "Bilecik"
                        },
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Email.ToString(),
                            ContactInformation = "caner@kisalar.net"
                        }
                    }
                },
                new Models.Phonebook()
                {
                    Id = Guid.NewGuid(),
                    Company = "TestCompany2 - TempCreate",
                    Surname = "TestSurname2 - TempCreate",
                    Name = "TestName2 - TempCreate",
                    Contacts = new List<Contact>()
                    {
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Phone.ToString(),
                            ContactInformation = "1234567890"
                        },
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Location.ToString(),
                            ContactInformation = "Eskiþehir"
                        },
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Email.ToString(),
                            ContactInformation = "caner@ccc.net"
                        }
                    }
                },
                new Models.Phonebook()
                {
                    Id = Guid.NewGuid(),
                    Company = "TestCompany3 - TempCreate",
                    Surname = "TestSurname3 - TempCreate",
                    Name = "TestName3 - TempCreate",
                    Contacts = new List<Contact>()
                    {
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Phone.ToString(),
                            ContactInformation = "1234567890"
                        },
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Location.ToString(),
                            ContactInformation = "Ankara"
                        },
                        new Contact()
                        {
                            Id = Guid.NewGuid(),
                            ContactType = ContactTypes.Email.ToString(),
                            ContactInformation = "caner@cccx.net"
                        }
                    }
                }
            });

            foreach (var phone in returnList)
            {
                _phonebookDal.Add(phone);
            }

            return returnList;

        }


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

        private static PhonebookService GetHandler()
        {
            var phonebookService = new PhonebookService(AutoMapperConfiguration.SetMapper(),
                SendEndpointFactory.GetSendEndpoint().Object, _phonebookDal);
            return phonebookService;
        }

        [Test]
        public async Task t010_CreatePhonebookTest()
        {
            var handler = GetHandler();

            var phonebookCreateDto = new PhonebookCreateDto()
            {
                Company = "TestCompany - Create",
                Surname = "TestSurname - Create",
                Name = "TestName - Create",
                Contacts = new List<ContactCreateDto>()
                {
                    new ContactCreateDto()
                    {
                        ContactType = ContactTypes.Phone.ToString(),
                        ContactInformation = "1234567890"
                    },
                    new ContactCreateDto()
                    {
                        ContactType = ContactTypes.Location.ToString(),
                        ContactInformation = "Bilecik"
                    },
                    new ContactCreateDto()
                    {
                        ContactType = ContactTypes.Email.ToString(),
                        ContactInformation = "caner@kisalar.net"
                    }
                }
            };

            var result = await handler.CreateAsync(phonebookCreateDto);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Company.Should().Be(phonebookCreateDto.Company);
            result.Data.Name.Should().Be(phonebookCreateDto.Name);
            result.Data.Surname.Should().Be(phonebookCreateDto.Surname);


        }

        [Test]
        public async Task t020_DeletePhonebookTest()
        {
            var handler = GetHandler();

            var phonebookCreate = PhonebookCreator();
            var entity = await _phonebookDal.Add(phonebookCreate);
            entity.Id.Should().NotBeEmpty();
            var result = await handler.DeleteAsync(entity.Id);
            result.IsSuccessful.Should().BeTrue();

            var deletedEntity = await _phonebookDal.Get(x => x.Id == entity.Id);

            deletedEntity.Should().BeNull();


        }

        [Test]
        public async Task t030_AddPhonebookContactTest()
        {
            var handler = GetHandler();

            var phonebookCreate = PhonebookCreator();
            var entity = await _phonebookDal.Add(phonebookCreate);
            entity.Id.Should().NotBeEmpty();
            var result = await handler.AddNewContactAsync(new ContactInsertDto()
            {
                ContactType = "Test",
                ContactInformation = "Test",
                PhonebookId = entity.Id
            });
            result.IsSuccessful.Should().BeTrue();

            var updatedPhonebook = await _phonebookDal.Get(x => x.Id == entity.Id);

            updatedPhonebook.Should().NotBeNull();
            updatedPhonebook.Contacts.Any(x => x.ContactType == "Test").Should().BeTrue();
            updatedPhonebook.Contacts.Count.Should().Be(4);


        }

        [Test]
        public async Task t040_DeletePhonebookContactTest()
        {
            var handler = GetHandler();

            var phonebookCreate = PhonebookCreator();
            var entity = await _phonebookDal.Add(phonebookCreate);
            entity.Id.Should().NotBeEmpty();
            var result = await handler.DeleteContactAsync(new ContactDeleteDto()
            {
                PhonebookId = entity.Id,
                ContactId = entity.Contacts[0].Id
            });
            result.IsSuccessful.Should().BeTrue();

            var updatedPhonebook = await _phonebookDal.Get(x => x.Id == entity.Id);

            updatedPhonebook.Should().NotBeNull();
            updatedPhonebook.Contacts.FirstOrDefault(c => c.Id == entity.Contacts[0].Id).Should().BeNull();
            updatedPhonebook.Contacts.Count.Should().Be(2);

        }

        [Test]
        public async Task t050_GetAllDataTest()
        {
            var handler = GetHandler();

            var phonebookCreateMany = PhonebookCreatorMany();
            
            var result = await handler.GetAllAsync();
            result.IsSuccessful.Should().BeTrue();
            result.Data.Count.Should().Be(3);

        }

        [Test]
        public async Task t060_GetAllDataWithDetailsTest()
        {
            var handler = GetHandler();

            var phonebookCreateMany = PhonebookCreatorMany();

            var result = await handler.GetAllWithDetailAsync();
            result.IsSuccessful.Should().BeTrue();
            result.Data.Count.Should().Be(3);
            result.Data.SelectMany(c => c.Contacts).Any().Should().BeTrue();
            result.Data.SelectMany(c => c.Contacts).Count().Should().Be(9);

        }

        [Test]
        public async Task t070_GetDetailByIdTest()
        {
            var handler = GetHandler();

            var entity = PhonebookCreator();

            var addedEntity = await _phonebookDal.Add(entity);

            var result = await handler.GetByIdAsync(entity.Id);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Id.Should().Be(addedEntity.Id);
            result.Data.Name.Should().Be(addedEntity.Name);
            result.Data.Surname.Should().Be(addedEntity.Surname);
            result.Data.Company.Should().Be(addedEntity.Company);
            
        }

        [Test]
        public async Task t080_GetByIdDetailsTest()
        {
            var handler = GetHandler();

            var entity = PhonebookCreator();

            var addedEntity = await _phonebookDal.Add(entity);

            var result = await handler.GetWithDetailByIdAsync(entity.Id);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Id.Should().Be(addedEntity.Id);
            result.Data.Contacts.Should().NotBeNull();
            result.Data.Contacts.Should().NotBeEmpty();

            for (var index = 0; index < result.Data.Contacts.Count; index++)
            {
                var contact = result.Data.Contacts[index];
                contact.ContactType.Should().Be(addedEntity.Contacts[index].ContactType);
                contact.ContactInformation.Should().Be(addedEntity.Contacts[index].ContactInformation);
            }
        }

        [Test]
        public async Task t080_SendDataToReportServiceTest()
        {
            var handler = GetHandler();

            var entities = PhonebookCreatorMany();
            var newGuid = Guid.NewGuid();
            var jsonEntites = JsonConvert.SerializeObject(entities);

            handler.SendDataToReportService(jsonEntites,newGuid);

            
        }

    }
}