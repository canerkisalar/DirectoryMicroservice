using System;
using System.Collections.Generic;
using Phonebook.Core.Domain;

namespace Phonebook.Services.DataCapture.Domain
{
    public class Phonebook : IEntity    
    {
        public Guid PhonebookId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<Contact> Contacts { get; set; } 

      
    }
}
