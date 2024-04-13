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
        public async Task<Guid> AddUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO, Guid orderId)
        {
            var existingOrderDetail = await Repository.GetOneAsync(x => x.ProductID == orderDetailDTO.ProductID && x.OrderID == orderId);

            if (existingOrderDetail == null)
            {
                var newOrderDetail = new OrderDetail
                {
                    OrderID = orderId,
                    ProductID = orderDetailDTO.ProductID,
                    Image = orderDetailDTO.Image,
                    NameProduct = orderDetailDTO.NameProduct,
                    Quantity = orderDetailDTO.Quantity,
                    Price = orderDetailDTO.Price,
                    Total = orderDetailDTO.Quantity * orderDetailDTO.Price,
                };
                Repository.Add(newOrderDetail);
            }
            else
            {
                existingOrderDetail.Quantity += orderDetailDTO.Quantity;
                existingOrderDetail.Total = existingOrderDetail.Quantity * orderDetailDTO.Price;
                Repository.Update(existingOrderDetail);
            }

            await UnitOfWork.SaveChangesAsync();
            return existingOrderDetail?.RecID ?? Guid.Empty;
        }

        public async Task<bool> DeleteOrderDetailsByOrder(Guid orderId)
        {
            var orderDetail = await Repository.GetOneAsync(x => x.OrderID == orderId);
            if (orderDetail != null)
            {
                Repository.Delete(orderDetail);
                await UnitOfWork.SaveChangesAsync();
            }
            return true;
        }
        public async Task<double> DeleteOrderDetailsAsync(Guid orderDetailProductID, Guid orderId)
        {
            var orderDetail = await Repository.GetOneAsync(x => x.ProductID == orderDetailProductID && x.OrderID == orderId);
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
        public async Task<List<OrderDetailDTO>> GetOrderDetailsInRange(DateTime fromDate, DateTime toDate)
        {
            var listOrderDetails = await Repository.GetAsync(x => (fromDate.Day <= x.CreatedOn.Day || fromDate.Month <= x.CreatedOn.Month || fromDate.Year <= x.CreatedOn.Year) && (x.CreatedOn.Day <= toDate.Day || x.CreatedOn.Month <= toDate.Month || x.CreatedOn.Year <= toDate.Year)).ToListAsync();
            return JsonConvert.DeserializeObject<List<OrderDetailDTO>>(JsonConvert.SerializeObject(listOrderDetails));
        }
        public async Task<List<OrderDetailDTO>> GetOrderDetailsByYear(int year)
        {
            var listOrderDetails = await Repository.GetAsync(x => x.CreatedOn.Year == year).ToListAsync();
            return JsonConvert.DeserializeObject<List<OrderDetailDTO>>(JsonConvert.SerializeObject(listOrderDetails));
        }

        public async Task<List<OrderDetailDTO>> GetOrderDetailsBy10Year()
        {
            int currentYear = DateTime.Now.Year;
            var listOrderDetails = await Repository.GetAsync(x => x.CreatedOn.Year >= currentYear - 10 && x.CreatedOn.Year <= currentYear).ToListAsync();
            return JsonConvert.DeserializeObject<List<OrderDetailDTO>>(JsonConvert.SerializeObject(listOrderDetails));
        }
    }
}
