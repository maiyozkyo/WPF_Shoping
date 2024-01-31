using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public class Enum
    {
        public enum PaymentStatus {
            Unpaid = 0,
            Paid = 1,
            InDebt = 2,
        }

        public enum DeliveryProcess
        {
            OrderCreated = 0,
            InTransti = 1,
            Delivery = 2,
            Delivered = 3,
        }
    }
}
