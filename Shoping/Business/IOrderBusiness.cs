using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public interface IOrderBusiness
    {
        public Task<bool> AddUpdateOrderAsync(OrderDTO orderDTO);
        public Task<bool> DeleteOrderAsync(Guid orderID);
        public Task<OrderDTO> GetOrderAsync(Guid orderID);
        public Task<List<OrderDTO>> GetOrdersInRangeAsync(DateTime from, DateTime to);
    }
}
