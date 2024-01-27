using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class MongoDBEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [Key]
        public string ID { get; set; }

        public Guid RecID { get; set; }

        public int Index { get; set; }

        public MongoDBEntity()
        {
            RecID = Guid.NewGuid();
            ID = RecID.ToString();
        }
    }
}
