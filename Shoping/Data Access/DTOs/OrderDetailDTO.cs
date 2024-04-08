namespace Shoping.Data_Access.DTOs
{
    public class OrderDetailDTO
    {
        public Guid RecID { get; set; }
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public string NameProduct { get; set; }
        public string Image { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
