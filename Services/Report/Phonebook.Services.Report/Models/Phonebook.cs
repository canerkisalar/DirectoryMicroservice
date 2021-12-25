using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Phonebook.Core.Domain;

namespace Phonebook.Services.Report.Models
{
    public class Phonebook : IEntity    
    {
        [JsonProperty("Id")]
        public Guid PhonebookId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<Contact> Contacts { get; set; } 

      
    }
}
