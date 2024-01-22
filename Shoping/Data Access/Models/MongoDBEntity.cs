using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class MongoDBEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        protected string _id;

        public string RecID { get; set; }
        public int Index { get; set; }

        public MongoDBEntity()
        {
            RecID = Guid.NewGuid().ToString();
        }
    }
}
