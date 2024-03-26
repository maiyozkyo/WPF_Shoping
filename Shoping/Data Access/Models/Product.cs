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
        public string Name { get; set; }
        public decimal Price { get; set; } //Giá bán
        public string Image { get; set; } // Implement later
        public Guid CatID { get; set; } //Category RecID
        public decimal PurchasePrice { get; set; } //Giá mua
        public int Quantity { get; set; } //Số lượng nhập hàng
    }
}
