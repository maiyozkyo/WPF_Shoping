using Shoping.Data_Access.DTOs;

namespace Shoping.Business.OrderDetailServices
{
    public interface IOrderDetailBusiness
    {
        public Task<Guid> AddUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO, Guid orderId);
        public Task<double> DeleteOrderDetailsAsync(Guid orderDetailRecID, Guid orderId);
        public Task<bool> DeleteOrderDetailsByOrder(Guid orderId);
        public Task<List<OrderDetailDTO>> GetAllOrderDetails(Guid orderId);
        public Task<(List<List<ChartItemDTO>>, List<string>)> GetSaleVolumnInDateRangeAsync(DateTime from, DateTime to);
        public Task<List<List<ChartItemDTO>>> GetSaleVolumnByWeekAsync(int year);
        public Task<List<List<ChartItemDTO>>> GetSaleVolumnByMonthAsync(int year);
        public Task<List<List<ChartItemDTO>>> GetSaleVolumnByYearAsync();
    }
}
