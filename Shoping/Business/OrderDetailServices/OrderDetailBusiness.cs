using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System.Collections.Generic;

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

        public async Task<(List<List<ChartItemDTO>>, List<string>)> GetSaleVolumnInDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var listOrderDetails = await Repository.GetAsync(x => fromDate.Date <= x.CreatedOn.Date && x.CreatedOn.Date <= toDate.Date).ToListAsync();
            var listOrderDetailsByDate = listOrderDetails.ToLookup(x => DateOnly.FromDateTime(x.CreatedOn));
            var dates = listOrderDetailsByDate.Select(x => x.Key).OrderBy(x => x).ToList().ConvertAll(x => x.ToString()[..10]);

            List<List<ChartItemDTO>> listProductsByDate = [];
            foreach (var products in listOrderDetailsByDate)
            {
                var productsByID = products.ToLookup(x => x.ProductID);
                List<ChartItemDTO> chartItem = [];
                foreach (var product in productsByID)
                {
                    var soldQuantity = product.Sum(x => x.Quantity);
                    chartItem.Add(new ChartItemDTO
                    {
                        ColumnName = product.First().NameProduct,
                        Quantity = (int)soldQuantity,
                    });
                }
                listProductsByDate.Add(chartItem);
            }

            return (listProductsByDate, dates);
        }

        public async Task<List<List<ChartItemDTO>>> GetSaleVolumnByWeekAsync(int year)
        {
            var listOrderDetails = await Repository.GetAsync(x => x.CreatedOn.Year == year).ToListAsync();
            var listOrderDetailsByWeek = listOrderDetails.ToLookup(x => x.CreatedOn.DayOfYear / 7);

            var listProductsByWeek = new List<List<ChartItemDTO>>(53);
            foreach (var products in listOrderDetailsByWeek)
            {
                var productsByID = products.ToLookup(x => x.ProductID);
                List<ChartItemDTO> chartItem = [];
                foreach (var product in productsByID)
                {
                    var soldQuantity = product.Sum(x => x.Quantity);
                    chartItem.Add(new ChartItemDTO
                    {
                        ColumnName = product.First().NameProduct,
                        Quantity = (int)soldQuantity,
                    });
                }
                listProductsByWeek[products.Key] = chartItem;
            }

            return listProductsByWeek;
        }

        public async Task<List<List<ChartItemDTO>>> GetSaleVolumnByMonthAsync(int year)
        {
            var listOrderDetails = await Repository.GetAsync(x => x.CreatedOn.Year == year).ToListAsync();
            var listOrderDetailsByMonth = listOrderDetails.ToLookup(x => x.CreatedOn.Month);

            var listProductsByMonth = new List<List<ChartItemDTO>>(12);
            foreach (var products in listOrderDetailsByMonth)
            {
                var productsByID = products.ToLookup(x => x.ProductID);
                List<ChartItemDTO> chartItem = [];
                foreach (var product in productsByID)
                {
                    var soldQuantity = product.Sum(x => x.Quantity);
                    chartItem.Add(new ChartItemDTO
                    {
                        ColumnName = product.First().NameProduct,
                        Quantity = (int)soldQuantity,
                    });
                }
                listProductsByMonth[products.Key - 1] = chartItem;
            }

            return listProductsByMonth;
        }

        public async Task<List<List<ChartItemDTO>>> GetSaleVolumnByYearAsync()
        {
            var currentYear = DateTime.Today.Year;
            var listOrderDetails = await Repository.GetAsync(x => currentYear - 10 <= x.CreatedOn.Year && x.CreatedOn.Year <= currentYear).ToListAsync();
            var listOrderDetailsByYear = listOrderDetails.ToLookup(x => 10 - (currentYear - x.CreatedOn.Year));

            var listProductsByYear =  new List<List<ChartItemDTO>>(11);
            foreach (var products in listOrderDetailsByYear)
            {
                var productsByID = products.ToLookup(x => x.ProductID);
                List<ChartItemDTO> chartItem = [];
                foreach (var product in productsByID)
                {
                    var soldQuantity = product.Sum(x => x.Quantity);
                    chartItem.Add(new ChartItemDTO
                    {
                        ColumnName = product.First().NameProduct,
                        Quantity = (int)soldQuantity,
                    });
                }
                listProductsByYear[products.Key] = chartItem;
            }

            return listProductsByYear;
        }
    }
}
