using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.ProductServices
{
    public interface IProductBusiness
    {
        public Task<Guid> AddUpdateProductAsync(ProductDTO? productDTO);
        public Task<bool> DeleteProductAsync(Guid productRecID);
        public Task<ProductDTO> GetProductAsync(String name);
        public Task<List<ProductDTO>> GetProductsInRangeAsync(int pageSize, int pageNumber);
        public Task<List<ProductDTO>> GetProductsAsync(String Name);
    }
}
