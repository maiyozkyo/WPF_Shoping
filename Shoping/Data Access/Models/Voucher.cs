using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class Voucher : MongoDBEntity
    {
        public string Code { get; set; }
        public double Value { get; set; }
        public double Max { get; set; }
        public bool IsPercent { get; set; }
        public DateTime ExpiredOn { get; set; }
    }
}
