using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.ProductServices
{
    public class ProductBusiness : BaseBusiness<Product>, IProductBusiness
    {
        public ProductBusiness(string _dbName) : base(_dbName)
        {

        }
        public async Task<Guid> AddUpdateProductAsync(ProductDTO productDTO)
        {
            var product = await Repository.GetOneAsync(x => x.RecID == productDTO.RecID);
            if (product == null)
            {
                product = new Product
                {
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Image = productDTO.Image
                };
                Repository.Add(product);
            }
            else
            {
                product.Name = productDTO.Name;
                product.Price = productDTO.Price;
                product.Image = productDTO.Image;
                Repository.Update(product);
            }
            await UnitOfWork.SaveChangesAsync();
            return product.RecID;
        }
        public async Task<bool> DeleteProductAsync(Guid productRecID)
        {
            var product = await Repository.GetOneAsync(x => x.RecID == productRecID);
            if (product != null)
            {
                Repository.Delete(product);
                await UnitOfWork.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<ProductDTO>> GetSearchProductsAsync(String Name)
        {
            var products = await Repository.GetAsync(x => x.Name.Contains(Name)).ToListAsync();
            if (products != null)
            {
                return JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(products));
            }
            return null;
        }
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var products = await Repository.GetAsync(x => true).ToListAsync();
            if (products != null)
            {
                return JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(products));
            }
            return null;
        }

        public async Task<List<ProductDTO>> GetProductsInRangeAsync(int pageSize, int pageNumber)
        {
            var productOrders = await Repository.GetAsync(x => true).Skip((pageNumber - 1) * pageSize).ToListAsync();
            return JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(productOrders));
        }

        public async Task<List<ProductDTO>> GetListProductsByRecID(List<Guid> lstRecIDs)
        {
            var lstProducts = await Repository.GetAsync(x => lstRecIDs.Contains(x.RecID)).ToListAsync();
            return JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(lstProducts));
        }

        public async Task<List<int>> GetSpendingInDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var listProducts = await Repository.GetAsync(x => fromDate <= x.CreatedOn && x.CreatedOn <= toDate).ToListAsync();
            var productsByDateTime = listProducts.ToLookup(x => x.CreatedOn.Date);
            List<int> spendingInDateRange = [];
            List<string> dates = [];

            foreach (var dateTime in productsByDateTime.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)productsByDateTime[dateTime].Sum(x => x.PurchasePrice * x.Quantity);
                spendingInDateRange.Add(total);
                dates.Add(dateTime.ToString()[..10]);
            }
            return spendingInDateRange;
        }

        public async Task<List<int>> GetSpendingByWeekAsync(int year)
        {
            var listProducts = await Repository.GetAsync(x => x.CreatedOn.Year == year).ToListAsync();
            var productsByWeek = listProducts.ToLookup(x => x.CreatedOn.DayOfYear / 7);

            List<int> spendingByWeek = Enumerable.Repeat(0, 53).ToList();

            foreach (var week in productsByWeek.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)productsByWeek[week].Sum(x => x.PurchasePrice * x.Quantity);
                spendingByWeek[week] = total;
            }
            return spendingByWeek;
        }

        public async Task<List<int>> GetSpendingByMonthAsync(int year)
        {
            var listProducts = await Repository.GetAsync(x => x.CreatedOn.Year == year).ToListAsync();
            var productsByMonth = listProducts.ToLookup(x => x.CreatedOn.Month);
            List<int> spendingByMonth = Enumerable.Repeat(0, 12).ToList();

            foreach (var month in productsByMonth.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)productsByMonth[month].Sum(x => x.PurchasePrice * x.Quantity);
                spendingByMonth[month - 1] = total;
            }
            return spendingByMonth;
        }

        public async Task<List<int>> GetSpendingByYearAsync()
        {
            var currentYear = DateTime.Today.Year;
            var listProducts = await Repository.GetAsync(x => currentYear - 10 <= x.CreatedOn.Year && x.CreatedOn.Year <= currentYear).ToListAsync();
            var productsByYear = listProducts.ToLookup(x => 10 - (currentYear - x.CreatedOn.Year));

            List<int> spendingByYear = Enumerable.Repeat(0, 11).ToList();

            foreach (var year in productsByYear.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)productsByYear[year].Sum(x => x.PurchasePrice * x.Quantity);
                spendingByYear[year] = total;
            }
            return spendingByYear;
        }
    }
}
