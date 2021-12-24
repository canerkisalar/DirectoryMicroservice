

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Directory.Services.Phonebook.Models
{
    public class Phonebook
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
