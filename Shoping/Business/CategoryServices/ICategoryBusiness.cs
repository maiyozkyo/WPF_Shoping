using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.CategoryServices
{
    public interface ICategoryBusiness
    {
        public Task<Guid> AddUpdateCategoryAsync(CategoryDTO categoryDTO);
        public Task<bool> DeleteCategoryAsync(Guid categoryRecID);
        public Task<List<CategoryDTO>> GetAllCategories();
        public Task<bool> DeleteAllCategories();
        public Task<Guid> GetCategoryID(string Name);
    }
}
