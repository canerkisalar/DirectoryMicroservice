using System;
using System.Collections.Generic;

namespace Phonebook.Services.Phonebook.Dtos.Phonebook
{
    public class PhonebookDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<Models.Contact> Contacts { get; set; }
    }
}
