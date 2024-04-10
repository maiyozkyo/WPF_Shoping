using Shoping.Data_Access.DTOs;

namespace Shoping.Business.OrderDetailServices
{
    public interface IOrderDetailBusiness
    {
        public Task<List<OrderDetailDTO>> GetOrderDetailsInRange(DateTime from, DateTime to);
        public Task<List<OrderDetailDTO>> GetOrderDetailsByYear(int year);
        public Task<List<OrderDetailDTO>> GetOrderDetailsBy10Year();

        public Task<Guid> AddUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO, Guid orderId);
        public Task<double> DeleteOrderDetailsAsync(Guid orderDetailRecID);
        public Task<List<OrderDetailDTO>> GetAllOrderDetails(Guid orderId);
    }
}
