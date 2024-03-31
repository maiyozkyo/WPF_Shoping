namespace Shoping.Data_Access.Models
{
    public class Order : MongoDBEntity
    {
        public Guid CustomerID { get; set; }
        public double TotalMoney { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public List<Guid> Vouchers { get; set; }
    }
}
