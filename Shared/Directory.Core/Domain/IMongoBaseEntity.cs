using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phonebook.Core.Domain
{
    public interface IMongoBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]

        public Guid Id { get; set; }
    }
}
