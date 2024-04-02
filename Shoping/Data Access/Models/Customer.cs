namespace Shoping.Data_Access.Models
{
    public class Customer : MongoDBEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} + {LastName}";
    }
}
