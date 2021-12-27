using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phonebook.Core.Domain;

namespace Phonebook.Services.Phonebook.Models
{
    public class Contact :IEntity,IMongoBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } 
        public string ContactType { get; set; }
        public string ContactInformation { get; set; }

    }
}
