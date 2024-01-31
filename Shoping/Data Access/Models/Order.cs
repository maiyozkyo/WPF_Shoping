using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class Order : MongoDBEntity
    {
        public Guid CustomerID { get; set; }
        public double Total { get; set; }
        public List<Guid> Vouchers { get; set; }
        public double Paid { get; set; }
        public int PaymentStatus { get; set; }
        public int LastestDeliveryStatus { get; set; }
    }
}
