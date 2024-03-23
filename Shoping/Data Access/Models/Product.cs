using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class Product : MongoDBEntity
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; } // Implement later
    }
}
