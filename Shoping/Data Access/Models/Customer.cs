namespace Shoping.Data_Access.Models
{
    public class Customer
    {
        public Guid RecID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} + {LastName}";
    }
}
