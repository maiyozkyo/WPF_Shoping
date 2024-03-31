namespace Shoping.Data_Access.DTOs
{
    public class CustomerDTO
    {
        public Guid RecID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} + {LastName}";
    }
}
