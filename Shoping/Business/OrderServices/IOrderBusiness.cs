using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

namespace Shoping.Business.OrderServices
{
    public interface IOrderBusiness
    {
        public Task<Guid> AddUpdateOrderAsync(OrderDTO orderDTO);
        public Task<bool> DeleteOrderAsync(Guid orderRecID);
        public Task<OrderDTO> GetOrderAsync(Guid orderRecID);
        public Task<PageData<OrderDTO>> GetOrdersPaging(int page, int pageSize);
        public Task<PageData<OrderDTO>> GetOrdersInRangeAsync(DateTime fromDate, DateTime toDate, int page, int pageSize);
        public Task<(List<int>, List<string>)> GetRevenueInDateRangeAsync(DateTime from, DateTime to);
        public Task<List<int>> GetRevenueByWeekAsync(int year);
        public Task<List<int>> GetRevenueByMonthAsync(int year);
        public Task<List<int>> GetRevenueByYearAsync();
    }
}
