using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

namespace Shoping.Business.CustomerServices
{
    public class CustomerBusiness : BaseBusiness<Customer>, ICustomerBusiness
    {
        public CustomerBusiness(string _dbName) : base(_dbName)
        {

        }
        public async Task<Guid> CreateCustomer(CustomerDTO customerDTO)
        {
            var customer = await Repository.GetOneAsync(x => x.RecID == customerDTO.RecID);
            if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = customerDTO.FirstName,
                    LastName = customerDTO.LastName,
                };
                Repository.Add(customer);
                await UnitOfWork.SaveChangesAsync();
            }
            return customer.RecID;
        }
        public async Task<CustomerDTO> GetCustomerById(Guid customerId)
        {
            var customer = await Repository.GetOneAsync(x => x.RecID == customerId);
            if (customer != null)
            {
                var customerDTO = new CustomerDTO
                {
                    RecID = customer.RecID,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };
                return customerDTO;
            }
            return null;
        }
    }
}
