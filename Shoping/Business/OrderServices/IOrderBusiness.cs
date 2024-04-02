using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

namespace Shoping.Business.OrderServices
{
    public interface IOrderBusiness
    {
        public Task<Guid> AddUpdateOrderAsync(OrderDTO orderDTO);
        public Task<bool> DeleteOrderAsync(Guid orderRecID);
        public Task<OrderDTO> GetOrderAsync(Guid orderRecID);
        public Task<List<OrderDTO>> GetOrdersInRangeAsync(DateTime from, DateTime to);
        public Task<List<OrderDTO>> GetAllOrders();
        public Task<PageData<OrderDTO>> GetOrderPaging(int page, int pageSize);
        public Task<Tuple<List<int>, List<string>>> GetRevenueInDateRangeAsync(DateTime from, DateTime to);
        public Task<List<int>> GetRevenueByWeekAsync(int year);
        public Task<List<int>> GetRevenueByMonthAsync(int year);
        public Task<List<int>> GetRevenueByYearAsync();
    }
}
