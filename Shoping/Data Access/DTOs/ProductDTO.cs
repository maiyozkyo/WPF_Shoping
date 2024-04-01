using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DTOs
{
    public class ProductDTO : ICloneable
    {
        public Guid RecID { get; set; }
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } // Implement later
        public Guid CatID { get; set; } 
        public decimal PurchasePrice { get; set; } 
        public int Quantity { get; set; } 
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
