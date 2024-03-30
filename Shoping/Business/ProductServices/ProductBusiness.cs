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
        public async Task<PageData<ProductDTO>> GetFilterProducts(String search, Guid CatID, int page, int pageSize)
        {
            if (CatID == Guid.Empty)
            {
                return await Repository.GetAsync(x => x.Name.Contains(search)).ToPaging<Product, ProductDTO>(page, pageSize);
            }
            else
            {
                var pageData = await Repository.GetAsync(x => x.Name.Contains(search) && x.CatID == CatID).ToPaging<Product, ProductDTO>(page, pageSize);
                return pageData;
            }
        }
    }
}
