using System;

namespace Phonebook.Services.Phonebook.Dtos.Phonebook
{
    public class PhonebookDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
    }
}

