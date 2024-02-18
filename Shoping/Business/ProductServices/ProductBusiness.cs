using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.ProductServices
{
    public class ProductBusiness : BaseBusiness<Product>, IProductServices
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
                await UnitOfWork.SaveChangesAsync();
            }
            else
            {

            }
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
        public async Task<ProductDTO> GetProductAsync(Guid productRecID)
        {
            var product = await Repository.GetOneAsync(x => x.RecID == productRecID);
            if (product != null)
            {
                return JsonConvert.DeserializeObject<ProductDTO>(JsonConvert.SerializeObject(product));
            }
            return null;
        }

        public async Task<List<ProductDTO>> GetProductsInRangeAsync(int pageSize, int pageNumber)
        {
            var productOrders = await Repository.GetAsync(x => true).Skip((pageNumber - 1) * pageSize).ToListAsync();
            return JsonConvert.DeserializeObject<List<ProductDTO>>(JsonConvert.SerializeObject(productOrders));
        }
    }
}
