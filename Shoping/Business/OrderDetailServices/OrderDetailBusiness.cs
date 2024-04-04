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

        public async Task<Guid> AddUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO, Guid orderId)
        {
            var orderDetail = await Repository.GetOneAsync(x => x.RecID == orderDetailDTO.RecID && x.CreatedBy == App.Auth.Email);
            if (orderDetail == null)
            {
                orderDetail = new OrderDetail
                {
                    OrderID = orderId,
                    ProductID = orderDetailDTO.ProductID,
                    Quantity = orderDetailDTO.Quantity,
                    Price = orderDetailDTO.Price,
                    Total = orderDetailDTO.Quantity * orderDetailDTO.Price,
                };
                Repository.Add(orderDetail);
            }
            else
            {
                orderDetail.OrderID = orderId;
                orderDetail.ProductID = orderDetailDTO.ProductID;
                orderDetail.Quantity = orderDetailDTO.Quantity;
                orderDetail.Price = orderDetailDTO.Price;
                orderDetail.Total = orderDetailDTO.Quantity * orderDetailDTO.Price;
                Repository.Update(orderDetail);
            }
            await UnitOfWork.SaveChangesAsync();
            return orderDetail.RecID;
        }

        public async Task<double> DeleteOrderDetailsAsync(Guid orderDetailRecID)
        {
            var orderDetail = await Repository.GetOneAsync(x => x.RecID == orderDetailRecID && x.CreatedBy == App.Auth.Email);
            double totalDeleted = 0;
            if (orderDetail != null)
            {
                totalDeleted = orderDetail.Total;
                Repository.Delete(orderDetail);
                await UnitOfWork.SaveChangesAsync();
            }
            return totalDeleted;
        }

        public async Task<List<OrderDetailDTO>> GetAllOrderDetails(Guid orderId)
        {
            var orderDetails = await Repository.GetAsync(x => x.OrderID == orderId).ToListAsync();
            if (orderDetails != null)
            {
                return JsonConvert.DeserializeObject<List<OrderDetailDTO>>(JsonConvert.SerializeObject(orderDetails));
            }
            return null;
        }
    }
}
