using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Directory.Core.Domain.Enums;
using Directory.Core.Messages.DataCapture;
using Directory.Services.DataCaptureService.Domain;
using Directory.Services.DataCaptureService.Infrastructure.Abstact;
using MassTransit;
using Contact = Directory.Services.DataCaptureService.Domain.Contact;

namespace Directory.Services.DataCaptureService.Consumers
{
    public class SnapPhonebookMessageCommandConsumer : IConsumer<SnapPhonebookMessageCommand>
    {
        private readonly IPhonebookDal _phonebookDal;
        private readonly IContactDal _contactDal;
        

        public SnapPhonebookMessageCommandConsumer(IPhonebookDal phonebookDal, IContactDal contactDal)
        {
            _phonebookDal = phonebookDal;
            _contactDal = contactDal;
        
        }

        public async Task Consume(ConsumeContext<SnapPhonebookMessageCommand> context)
        {
            if (context.Message != null)
            {
                switch (context.Message.Modification)
                {
                    case (int)ModiTypes.Create:

                        #region Fill Entity

                        var newPhonebook = new Phonebook()
                        {
                            PhonebookId = context.Message.Id,
                            Name = context.Message.Name,
                            Surname = context.Message.Surname,
                            Company = context.Message.Company,
                            Contacts = new List<Contact>()
                        };

                        foreach (var contact in context.Message.Contacts)
                        {
                            newPhonebook.Contacts.Add(new Contact()
                            {
                                ContactInformation = contact.ContactInformation,
                                ContactType = contact.ContactType,
                                ContactId = contact.Id
                            });
                        }

                        #endregion

                        await _phonebookDal.Add(newPhonebook);
                        break;
                    case (int)ModiTypes.Delete:
                        await _phonebookDal.Delete(context.Message.Id);
                        break;
                    case (int)ModiTypes.Update:

                        await _phonebookDal.Delete(context.Message.Id);

                        var updatedPhonebook = new Phonebook()
                        {
                            PhonebookId = context.Message.Id,
                            Name = context.Message.Name,
                            Surname = context.Message.Surname,
                            Company = context.Message.Company,
                            Contacts = new List<Contact>()
                        };

                        foreach (var contact in context.Message.Contacts)
                        {
                            updatedPhonebook.Contacts.Add(new Contact()
                            {
                                ContactInformation = contact.ContactInformation,
                                ContactType = contact.ContactType,
                                ContactId = contact.Id
                            });
                        }

                        await _phonebookDal.Add(updatedPhonebook);
                        break;

                }
                
            }
        }
    }
}
