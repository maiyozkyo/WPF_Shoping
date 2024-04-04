using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

namespace Shoping.Business.OrderDetailServices
{
    public class OrderDetailBusiness : BaseBusiness<OrderDetail>, IOrderDetailBusiness
    {
        public OrderDetailBusiness(string _dbName) : base(_dbName)
        {

        }
        // Order details
        public async Task<List<OrderDetailDTO>> GetOrderDetailsInRange(DateTime from, DateTime to)
        {
            var lstOrderDetails = await Repository.GetAsync(x => x.CreatedOn >= from && x.CreatedOn <= to && x.CreatedBy == App.Auth.Email).ToListAsync();
            return JsonConvert.DeserializeObject<List<OrderDetailDTO>>(JsonConvert.SerializeObject(lstOrderDetails));
        }

        public async Task<Guid> AddUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO, Guid productId)
        {
            var orderDetail = await Repository.GetOneAsync(x => x.RecID == orderDetailDTO.RecID && x.CreatedBy == App.Auth.Email);
            if (orderDetail == null)
            {
                orderDetail = new OrderDetail
                {
                    ProductID = productId,
                    Quantity = orderDetailDTO.Quantity,
                    Price = orderDetailDTO.Price,
                    Total = orderDetailDTO.Quantity * (double)orderDetailDTO.Price,
                };

                Repository.Add(orderDetail);
            }
            else
            {
                orderDetail.Quantity = orderDetailDTO.Quantity;
                orderDetail.Price = orderDetailDTO.Price;
                orderDetail.Total = orderDetailDTO.Quantity * (double)orderDetailDTO.Price;
            }
            await UnitOfWork.SaveChangesAsync();
            return orderDetail.RecID;
        }

        public async Task<bool> DeleteOrderDetailsAsync(Guid orderDetailRecID)
        {
            var order = await Repository.GetOneAsync(x => x.RecID == orderDetailRecID && x.CreatedBy == App.Auth.Email);
            if (order != null)
            {
                Repository.Delete(order);
                await UnitOfWork.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<OrderDetailDTO>> GetAllOrderDetails()
        {
            var orderDetails = await Repository.GetAsync(x => true).ToListAsync();
            if (orderDetails != null)
            {
                return JsonConvert.DeserializeObject<List<OrderDetailDTO>>(JsonConvert.SerializeObject(orderDetails));
            }
            return null;
        }
    }
}
