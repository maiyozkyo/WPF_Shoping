using PropertyChanged;
//using Shoping.Business.OderServices;
using Shoping.Business.ProductServices;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;

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
        public async Task<PageData<ProductDTO>> Paging(int page, int pageSize)
        {
            return await ProductBusiness.GetProductsPaging(page, pageSize);
        }
        public async Task<PageData<ProductDTO>> GetFilterProducts(String searchFilter, Guid CatID, int page, int pageSize)
        {
            return await ProductBusiness.GetFilterProducts(searchFilter, CatID, page, pageSize);
        }
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            return await ProductBusiness.GetAllProducts();
        }
    }
}
