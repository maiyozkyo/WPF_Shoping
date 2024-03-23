using Microsoft.EntityFrameworkCore.Query;
using PropertyChanged;
//using Shoping.Business.OderServices;
using Shoping.Business.ProductServices;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
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
        public async Task<PageData<ProductDTO>> SearchProduct(String searchName, int page, int pageSize)
        {
            var result = await ProductBusiness.GetSearchProductsAsync(searchName, page, pageSize);
            return result;
        }
        public async Task<PageData<ProductDTO>> Paging(int page, int pageSize)
        {
            return await ProductBusiness.GetProductsPaging(page, pageSize);
        }
    }
}
