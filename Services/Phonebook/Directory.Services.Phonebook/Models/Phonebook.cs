using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phonebook.Core.Domain;

namespace Phonebook.Services.Phonebook.Models
{
    public class Phonebook : IEntity, IMongoBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<Contact> Contacts { get; set; }

    }
}
