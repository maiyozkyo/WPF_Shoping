using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DTOs
{
    public class OrderDTO
    {
        public Guid RecID { get; set; }
        public Guid CustomerID { get; set; }
        public double Total { get; set; }
        public List<Guid> Vouchers { get; set; }
        public double Paid { get; set; }
        public int PaymentStatus { get; set; }
        public int LastestDeliveryStatus { get; set; }
        public string Image { get; set; }
    }
}
