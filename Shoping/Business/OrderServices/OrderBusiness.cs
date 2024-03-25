using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DB.Repo;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System.Windows.Controls;

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
                    Paid = orderDTO.Paid,
                    Total = orderDTO.Total,
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

        public async Task<PageData<OrderDTO>> GetOrderPaging(int page, int pageSize)
        {
            var pageData = await Repository.GetAsync(x => x.Paid < 39).ToPaging<Order, OrderDTO>(page, pageSize);
            return pageData;
        }
        public async Task<List<OrderDTO>> GetOrdersInRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var lstOrders = await Repository.GetAsync(x => fromDate <= x.CreatedOn && x.CreatedOn <= toDate).ToListAsync();
            return JsonConvert.DeserializeObject<List<OrderDTO>>(JsonConvert.SerializeObject(lstOrders));
        }
        public async Task<List<int>> GetRevenueByWeekAsync(int year)
        {
            var listOrders = await Repository.GetAsync(x => x.CreatedOn.Year == year).ToListAsync();
            var ordersByWeek = listOrders.ToLookup(x => x.CreatedOn.DayOfYear / 7);

            List<int> revenueByWeek = Enumerable.Repeat(0, 53).ToList();

            foreach (var week in ordersByWeek.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByWeek[week].Sum(x => x.Total * 1000);
                revenueByWeek[week] = total;
            }
            return revenueByWeek;
        }
        public async Task<List<int>> GetRevenueByMonthAsync(int year)
        {
            var listOrders = await Repository.GetAsync(x => x.CreatedOn.Year == year).ToListAsync();
            var ordersByMonth = listOrders.ToLookup(x => x.CreatedOn.Month);
            List<int> revenueByMonth = Enumerable.Repeat(0, 12).ToList();

            foreach (var month in ordersByMonth.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByMonth[month].Sum(x => x.Total * 1000);
                revenueByMonth[month - 1] = total;
            }
            return revenueByMonth;
        }
        public async Task<List<int>> GetRevenueByYearAsync()
        {
            var currentYear = DateTime.Today.Year;
            var listOrders = await Repository.GetAsync(x => currentYear - 10 <= x.CreatedOn.Year && x.CreatedOn.Year <= currentYear).ToListAsync();
            var ordersByYear = listOrders.ToLookup(x => 10 - (currentYear - x.CreatedOn.Year));

            List<int> revenueByYear = Enumerable.Repeat(0, 11).ToList();

            foreach (var year in ordersByYear.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByYear[year].Sum(x => x.Total * 1000);
                revenueByYear[year] = total;
            }
            return revenueByYear;
        }
        public async Task<Tuple<List<int>, List<string>>> GetRevenueInDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var listOrders = await Repository.GetAsync(x => fromDate <= x.CreatedOn && x.CreatedOn <= toDate).ToListAsync();
            var ordersByDateTime = listOrders.ToLookup(x => x.CreatedOn.Date);
            List<int> revenueInDateRange = [];
            List<string> dates = [];

            foreach (var dateTime in ordersByDateTime.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByDateTime[dateTime].Sum(x => x.Total * 1000);
                revenueInDateRange.Add(total);
                dates.Add(dateTime.ToString()[..10]);
            }
            return new Tuple<List<int>, List<string>>(revenueInDateRange, dates);
        }
    }
}
