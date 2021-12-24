
using System;
using System.Collections.Generic;
using Directory.Core.Domain;

namespace Directory.Services.DataCaptureService.Domain
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
