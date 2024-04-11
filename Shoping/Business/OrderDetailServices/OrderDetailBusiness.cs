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
            var orderDetail = await Repository.GetOneAsync(x => x.RecID == orderDetailDTO.RecID);
            if (orderDetail == null)
            {
                orderDetail = new OrderDetail
                {
                    OrderID = orderId,
                    ProductID = orderDetailDTO.ProductID,
                    Image = orderDetailDTO.Image,
                    NameProduct = orderDetailDTO.NameProduct,
                    Quantity = orderDetailDTO.Quantity,
                    Price = orderDetailDTO.Price,
                    Total = orderDetailDTO.Quantity * orderDetailDTO.Price,
                };
                Repository.Add(orderDetail);
            }
            else
            {
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
            var orderDetail = await Repository.GetOneAsync(x => x.RecID == orderDetailRecID);
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
