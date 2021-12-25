using System;

namespace Phonebook.Services.Phonebook.Dtos.Contact
{
    public class ContactInsertDto
    {
        public Guid PhonebookId { get; set; }
        public string ContactType { get; set; }
        public string ContactInformation { get; set; }
    }
}
