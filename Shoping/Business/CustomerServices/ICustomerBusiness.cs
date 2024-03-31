using Shoping.Data_Access.DTOs;

namespace Shoping.Business.CustomerServices
{
    public interface ICustomerBusiness
    {
        public Task<Guid> CreateCustomer(CustomerDTO customerDTO);
        public Task<CustomerDTO> GetCustomerById(Guid customerId);
    }
}
