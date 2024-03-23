﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<PageData<ProductDTO>> GetSearchProductsAsync(String Name, int page, int pageSize)
        {
            /*var products = await Repository.GetAsync(x => x.Name.Contains(Name)).ToListAsync();
            if (products != null)
            {
                return JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(products));
            }
            return null;*/

            var pageData = await Repository.GetAsync(x => x.Name.Contains(Name)).ToPaging<Product, ProductDTO>(page, pageSize);
            return pageData;
        }

        public async Task<PageData<ProductDTO>> GetProductsPaging(int page, int pageSize)
        {
            var pageData = await Repository.GetAsync(x => true).ToPaging<Product, ProductDTO>(page, pageSize);
            return pageData;
        }
    }
}
