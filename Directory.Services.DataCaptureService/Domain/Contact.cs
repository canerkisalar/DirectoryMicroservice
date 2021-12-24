using System;
using System.ComponentModel.DataAnnotations.Schema;
using Directory.Core.Domain;

namespace Directory.Services.DataCaptureService.Domain
{
    public class Contact : IEntity
    {

        public Guid ContactId { get; set; }
        public string ContactType { get; set; }
        public string ContactInformation { get; set; }
        
        public Guid PhonebookId { get; set; }
        [ForeignKey("PhonebookId")]
        public Phonebook Phonebook { get; set; }
    }
}
