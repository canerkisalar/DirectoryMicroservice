using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Phonebook.Core.Domain;

namespace Phonebook.Services.Report.Models
{
    public class Contact : IEntity
    {
        [JsonProperty("Id")]
        public Guid ContactId { get; set; }
        public string ContactType { get; set; }
        public string ContactInformation { get; set; }
        
        public Guid PhonebookId { get; set; }
        [ForeignKey("PhonebookId")]
        public Phonebook Phonebook { get; set; }
    }
}
