using System.Collections.Generic;
using Phonebook.Services.Phonebook.Dtos.Contact;

namespace Phonebook.Services.Phonebook.Dtos.Phonebook
{
    public class PhonebookCreateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<ContactCreateDto> Contacts { get; set; }
    }
}
