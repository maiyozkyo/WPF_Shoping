using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DB.Repo;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

namespace Shoping.Business.OrderServices
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
                    TotalMoney = orderDTO.TotalMoney,
                    DeliveryDate = orderDTO.DeliveryDate,
                    PaymentStatus = orderDTO.PaymentStatus,
                };

                Repository.Add(order);
            }
            else
            {
                order.TotalMoney = 1;
                order.DeliveryDate = orderDTO.DeliveryDate;
                order.PaymentStatus = orderDTO.PaymentStatus;
                Repository.Update(order);
            }
            await UnitOfWork.SaveChangesAsync();
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

        public async Task<List<OrderDTO>> GetAllOrders()
        {
            var orders = await Repository.GetAsync(x => true).ToListAsync();
            if (orders != null)
            {
                return JsonConvert.DeserializeObject<List<OrderDTO>>(JsonConvert.SerializeObject(orders));
            }
            return null;
        }

        public async Task<PageData<OrderDTO>> GetOrderPaging(int page, int pageSize)
        {
            var pageData = await Repository.GetAsync(x => true).ToPaging<Order, OrderDTO>(page, pageSize);
            return pageData;
        }

        public async Task<PageData<OrderDTO>> GetOrdersPaging(int page, int pageSize)
        {
            var pageData = await Repository.GetAsync(x => true).ToPaging<Order, OrderDTO>(page, pageSize);
            return pageData;
        }
    }
}
