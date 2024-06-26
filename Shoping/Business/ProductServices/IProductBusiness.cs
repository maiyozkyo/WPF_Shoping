﻿using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

namespace Shoping.Business.ProductServices
{
    public interface IProductBusiness
    {
        public Task<Guid> AddUpdateProductAsync(ProductDTO productDTO);
        public Task<bool> DeleteProductAsync(Guid productRecID);
        public Task<List<ProductDTO>> GetAllProducts();
        public Task<PageData<ProductDTO>> GetProductsPaging(int page, int pageSize);
        public Task<PageData<ProductDTO>> GetFilterProducts(String search, Guid CatID, double from, double to, int page, int pageSize);
        public Task<List<ProductDTO>> GetListProductsByRecID(List<Guid> lstRecIDs);
        public Task<(List<int>, List<string>)> GetSpendingInDateRangeAsync(DateTime from, DateTime to);
        public Task<List<int>> GetSpendingByWeekAsync(int year);
        public Task<List<int>> GetSpendingByMonthAsync(int year);
        public Task<List<int>> GetSpendingByYearAsync();
        public Task<bool> DeleteAllProducts();
        public Task<bool> CheckProductCategory(Guid category);
    }
}
