using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DB.Repo;
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
            var product = await Repository.GetOneAsync(x => x.RecID == productDTO.RecID && x.CreatedBy == App.Auth.UserName);
            if (product == null)
            {
                product = new Product
                {
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    PurchasePrice = productDTO.PurchasePrice,
                    CatID = productDTO.CatID,
                    Quantity = productDTO.Quantity,
                    Image = productDTO.Image
                };
                Repository.Add(product);
            }
            else
            {
                product.Name = productDTO.Name;
                product.Price = productDTO.Price;
                product.PurchasePrice = productDTO.PurchasePrice;
                product.CatID = productDTO.CatID;
                product.Quantity = productDTO.Quantity;
                product.Image = productDTO.Image;
                Repository.Update(product);
            }
            await UnitOfWork.SaveChangesAsync();
            return product.RecID;
        }
        public async Task<bool> DeleteProductAsync(Guid productRecID)
        {
            var product = await Repository.GetOneAsync(x => x.RecID == productRecID && x.CreatedBy == App.Auth.UserName);
            if (product != null)
            {
                Repository.Delete(product);
                await UnitOfWork.SaveChangesAsync();
            }
            return true;
        }

        public async Task<PageData<ProductDTO>> GetProductsPaging(int page, int pageSize)
        {
            var pageData = await Repository.GetAsync(x => true).ToPaging<Product, ProductDTO>(page, pageSize);
            return pageData;
        }
        public async Task<PageData<ProductDTO>> GetFilterProducts(String search, Guid CatID, int page, int pageSize)
        {
            if (CatID == Guid.Empty)
            {
                return await Repository.GetAsync(x => x.Name.Contains(search) && x.CreatedBy == App.Auth.UserName).ToPaging<Product, ProductDTO>(page, pageSize);
            }
            else
            {
                var pageData = await Repository.GetAsync(x => x.Name.Contains(search) && x.CatID == CatID && x.CreatedBy == App.Auth.UserName).ToPaging<Product, ProductDTO>(page, pageSize);
                return pageData;
            }
        }

        public async Task<List<ProductDTO>> GetListProductsByRecID(List<Guid> lstRecIDs)
        {
            var lstProducts = await Repository.GetAsync(x => lstRecIDs.Contains(x.RecID) && x.CreatedBy == App.Auth.UserName).ToListAsync();
            return JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(lstProducts));
        }

        public async Task<Tuple<List<int>, List<string>>> GetSpendingInDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var listProducts = await Repository.GetAsync(x => fromDate <= x.CreatedOn && x.CreatedOn <= toDate && x.CreatedBy == App.Auth.UserName).ToListAsync();
            var productsByDateTime = listProducts.ToLookup(x => x.CreatedOn.Date);
            List<int> spendingInDateRange = [];
            List<string> dates = [];

            foreach (var dateTime in productsByDateTime.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)productsByDateTime[dateTime].Sum(x => x.PurchasePrice * x.Quantity);
                spendingInDateRange.Add(total);
                dates.Add(dateTime.ToString()[..10]);
            }
            return new Tuple<List<int>, List<string>>(spendingInDateRange, dates);
        }

        public async Task<List<int>> GetSpendingByWeekAsync(int year)
        {
            var listProducts = await Repository.GetAsync(x => x.CreatedOn.Year == year && x.CreatedBy == App.Auth.UserName).ToListAsync();
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
            var listProducts = await Repository.GetAsync(x => x.CreatedOn.Year == year && x.CreatedBy == App.Auth.UserName).ToListAsync();
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
            var listProducts = await Repository.GetAsync(x => currentYear - 10 <= x.CreatedOn.Year && x.CreatedOn.Year <= currentYear && x.CreatedBy == App.Auth.UserName).ToListAsync();
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
