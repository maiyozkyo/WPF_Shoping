using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class DeliveryProcess
    {
        public Guid OrderID { get; set; }
        public int Status { get;set; }
    }
}
