﻿using Microsoft.EntityFrameworkCore;
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
        // Order
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
                order.TotalMoney = orderDTO.TotalMoney;
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

        public async Task<PageData<OrderDTO>> GetOrdersPaging(int page, int pageSize)
        {
            var pageData = await Repository.GetAsync(x => true).OrderByDescending(x => x.CreatedOn).ToPaging<Order, OrderDTO>(page, pageSize);
            return pageData;
        }

        public async Task<PageData<OrderDTO>> GetOrdersInRangeAsync(DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            var pageData = await Repository.GetAsync(x => fromDate <= x.CreatedOn && x.CreatedOn <= toDate).ToPaging<Order, OrderDTO>(page, pageSize);
            return pageData;
        }
        // Report
        public async Task<List<int>> GetRevenueByWeekAsync(int year)
        {
            var listOrders = await Repository.GetAsync(x => x.CreatedOn.Year == year && x.PaymentStatus == true).ToListAsync();
            var ordersByWeek = listOrders.ToLookup(x => x.CreatedOn.DayOfYear / 7);

            List<int> revenueByWeek = Enumerable.Repeat(0, 53).ToList();

            foreach (var week in ordersByWeek.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByWeek[week].Sum(x => x.TotalMoney);
                revenueByWeek[week] = total;
            }
            return revenueByWeek;
        }

        public async Task<List<int>> GetRevenueByMonthAsync(int year)
        {
            var listOrders = await Repository.GetAsync(x => x.CreatedOn.Year == year && x.PaymentStatus == true).ToListAsync();
            var ordersByMonth = listOrders.ToLookup(x => x.CreatedOn.Month);
            List<int> revenueByMonth = Enumerable.Repeat(0, 12).ToList();

            foreach (var month in ordersByMonth.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByMonth[month].Sum(x => x.TotalMoney);
                revenueByMonth[month - 1] = total;
            }
            return revenueByMonth;
        }

        public async Task<List<int>> GetRevenueByYearAsync()
        {
            var currentYear = DateTime.Today.Year;
            var listOrders = await Repository.GetAsync(x => currentYear - 10 <= x.CreatedOn.Year && x.CreatedOn.Year <= currentYear && x.PaymentStatus == true).ToListAsync();
            var ordersByYear = listOrders.ToLookup(x => 10 - (currentYear - x.CreatedOn.Year));

            List<int> revenueByYear = Enumerable.Repeat(0, 11).ToList();

            foreach (var year in ordersByYear.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByYear[year].Sum(x => x.TotalMoney);
                revenueByYear[year] = total;
            }
            return revenueByYear;
        }

        public async Task<(List<int>, List<string>)> GetRevenueInDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var listOrders = await Repository.GetAsync(x => fromDate.Date <= x.CreatedOn.Date && x.CreatedOn.Date <= toDate.Date && x.PaymentStatus == true).ToListAsync();
            var ordersByDateTime = listOrders.ToLookup(x => DateOnly.FromDateTime(x.CreatedOn));
            List<int> revenueInDateRange = [];
            List<string> dates = [];

            foreach (var dateTime in ordersByDateTime.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)ordersByDateTime[dateTime].Sum(x => x.TotalMoney);
                revenueInDateRange.Add(total);
                dates.Add(dateTime.ToString());
            }
            return (revenueInDateRange, dates);
        }
    }
}
