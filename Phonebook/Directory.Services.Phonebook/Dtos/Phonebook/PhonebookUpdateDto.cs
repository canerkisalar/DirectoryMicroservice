using System;
using System.Collections.Generic;

namespace Directory.Services.Phonebook.Dtos.Phonebook
{
    public class PhonebookUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<Models.Contact> Contact { get; set; }
    }
}
