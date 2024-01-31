using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public class OrderBusiness : BaseBusiness<Order>, IOrderBusiness
    {
        public OrderBusiness(string _dbName) : base(_dbName)
        {

        }
        public async Task<bool> AddUpdateOrderAsync(OrderDTO orderDTO)
        {
            var order = await Repository.GetOneAsync(x => x.RecID == orderDTO.RecID);
            if (order == null)
            {
                order = new Order
                {
                    CustomerID = orderDTO.CustomerID,
                    CreatedBy = Auth
                }
            }
            else
            {

            }
        }

        public async Task<bool> DeleteOrderAsync(Guid orderID)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDTO> GetOrderAsync(Guid orderID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDTO>>GetOrdersInRangeAsync(DateTime from, DateTime to)
        
            throw new NotImplementedException();
        }
    }
}
