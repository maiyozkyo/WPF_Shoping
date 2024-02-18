using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class OrderDetail : MongoDBEntity
    {
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public double Total { get; set; }
    }
}

