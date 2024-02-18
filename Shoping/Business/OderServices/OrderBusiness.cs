using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

namespace Shoping.Business.OderServices
{
    public class OrderBusiness : BaseBusiness<Order>, IOrderBusiness
    {
        public OrderBusiness(string _dbName) : base(_dbName)
        {

        }
        public async Task<Guid> AddUpdateOrderAsync(OrderDTO orderDTO)
        {
            var order = await Repository.GetOneAsync(x => x.RecID == orderDTO.RecID);
            if (order == null)
            {
                order = new Order
                {
                    CustomerID = orderDTO.CustomerID,
                };
                Repository.Add(order);
                await UnitOfWork.SaveChangesAsync();
            }
            else
            {
                
            }
            return order.RecID;
        }

        public async Task<bool> DeleteOrderAsync(Guid orderRecID)
        {
            var order = await Repository.GetOneAsync(x => x.RecID == orderRecID);
            if (order != null)
            {
                Repository.Delete(order);
                await UnitOfWork.SaveChangesAsync();
            }
            return true;
        }

        public async Task<OrderDTO> GetOrderAsync(Guid orderRecID)
        {
            var order = await Repository.GetOneAsync(x => x.RecID == orderRecID);
            if (order != null)
            {
                return JsonConvert.DeserializeObject<OrderDTO>(JsonConvert.SerializeObject(order));
            }
            return null;
        }

        public async Task<List<OrderDTO>> GetOrdersInRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var lstOrders = await Repository.GetAsync(x => fromDate <= x.CreatedOn && x.CreatedOn <= toDate).ToListAsync();
            
            return JsonConvert.DeserializeObject<List<OrderDTO>>(JsonConvert.SerializeObject(lstOrders));
        }
    }
}
