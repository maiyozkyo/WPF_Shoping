using Microsoft.EntityFrameworkCore.Query;
using PropertyChanged;
//using Shoping.Business.OderServices;
using Shoping.Business.ProductServices;
using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel 
    {
        public IProductBusiness ProductBusiness;
        public List<ProductDTO> products { get; set; }
        public MainViewModel(IProductBusiness productBusiness)
        {
            ProductBusiness = productBusiness;
        }

        public async Task<Guid> AddUpdateProduct(ProductDTO productDTO)
        {
            var result = await ProductBusiness.AddUpdateProductAsync(productDTO);
            return result;
        }

        public async Task<bool> DeleteProduct(ProductDTO productDTO)
        {
            var result = await ProductBusiness.DeleteProductAsync(productDTO.RecID);
            return result;
        }
        public async Task<List<ProductDTO>> SearchProduct(String searchName)
        {
            var result = await ProductBusiness.GetSearchProductsAsync(searchName);
            return result;
        }
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            products = await ProductBusiness.GetAllProducts();
            return products;
        }
    }
}
