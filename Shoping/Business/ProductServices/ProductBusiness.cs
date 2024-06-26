﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DB.Repo;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

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
            var product = await Repository.GetOneAsync(x => x.RecID == productRecID);
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
        public async Task<PageData<ProductDTO>> GetFilterProducts(String search, Guid CatID, double from, double to, int page, int pageSize)
        {
            if (CatID == Guid.Empty)
            {
                return await Repository.GetAsync(x => x.Name.ToLower().Contains(search.ToLower()) && (x.Price >= from && x.Price <= to)).ToPaging<Product, ProductDTO>(page, pageSize);
            }
            else
            {
                var pageData = await Repository.GetAsync(x => x.Name.ToLower().Contains(search.ToLower()) && x.CatID == CatID && (x.Price >= from && x.Price <= to)).ToPaging<Product, ProductDTO>(page, pageSize);
                return pageData;
            }
        }

        public async Task<List<ProductDTO>> GetListProductsByRecID(List<Guid> lstRecIDs)
        {
            var lstProducts = await Repository.GetAsync(x => lstRecIDs.Contains(x.RecID)).ToListAsync();
            var lstProductDTOs = JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(lstProducts));
            return lstProductDTOs;
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

        public async Task<(List<int>, List<string>)> GetSpendingInDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var listProducts = await Repository.GetAsync(x => fromDate.Date <= x.CreatedOn.Date && x.CreatedOn.Date <= toDate.Date).ToListAsync();
            var productsByDateTime = listProducts.ToLookup(x => DateOnly.FromDateTime(x.CreatedOn));
            List<int> spendingInDateRange = [];
            List<string> dates = [];

            foreach (var dateTime in productsByDateTime.Select(x => x.Key).OrderBy(x => x))
            {
                var total = (int)productsByDateTime[dateTime].Sum(x => x.PurchasePrice * x.Quantity);
                spendingInDateRange.Add(total);
                dates.Add(dateTime.ToString());
            }
            return (spendingInDateRange, dates);
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

        public async Task<bool> DeleteAllProducts()
        {
            var products = await Repository.GetAsync(x => true).ToListAsync();
            if (products != null)
            {
                Repository.Delete(products);
                await UnitOfWork.SaveChangesAsync();
            }
            return true;
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
        public async Task<bool> CheckProductCategory(Guid category)
        {
            var product = await Repository.GetOneAsync(x => x.CatID == category);
            if (product != null)
            {
                return true;
            }
            return false;
        }
    }
}
