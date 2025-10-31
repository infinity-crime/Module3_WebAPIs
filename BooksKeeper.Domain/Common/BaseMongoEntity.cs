using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Common
{
    public abstract class BaseMongoEntity<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public T Id { get; protected set; }
    }
}
