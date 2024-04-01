using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class Category : MongoDBEntity
    {
        public string Name { get; set; }
    }
}
