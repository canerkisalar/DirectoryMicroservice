
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Directory.Services.Phonebook.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]

        public Guid Id { get; set; } 
        public string ContactType { get; set; }
        public string ContactInformation { get; set; }
    }
}
