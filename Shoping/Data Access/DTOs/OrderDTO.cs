namespace Shoping.Data_Access.DTOs
{
    public class OrderDTO
    {
        public Guid RecID { get; set; }
        public Guid CustomerID { get; set; }
        public double TotalMoney { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool PaymentStatus { get; set; }
        public List<Guid> Vouchers { get; set; }
    }
}
