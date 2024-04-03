using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DTOs
{
    public class ProductExcelDTO : ICloneable
    {
        public Guid RecID { get; set; }
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; } // Implement later
        public string CatID { get; set; }
        public double PurchasePrice { get; set; }
        public int Quantity { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
