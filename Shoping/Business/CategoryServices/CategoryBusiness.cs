using Microsoft.EntityFrameworkCore;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shoping.Business.CategoryServices
{
    public class CategoryBusiness : BaseBusiness<Category>, ICategoryBusiness
    {
        public CategoryBusiness(string _dbName) : base(_dbName)
        {

        }
        public async Task<Guid> AddUpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            var category = await Repository.GetOneAsync(x => x.RecID == categoryDTO.RecID && x.CreatedBy == App.Auth.UserName);
            if (category == null)
            {
                category = new Category
                {
                    Name = categoryDTO.Name,
                };
                Repository.Add(category);
            }
            else
            {
                category.Name = categoryDTO.Name;
                Repository.Update(category);
            }
            await UnitOfWork.SaveChangesAsync();
            return category.RecID;
        }
        public async Task<bool> DeleteCategoryAsync(Guid categoryRecID)
        {
            var category = await Repository.GetOneAsync(x => x.RecID == categoryRecID && x.CreatedBy == App.Auth.UserName);
            if (category != null)
            {
                Repository.Delete(category);
                await UnitOfWork.SaveChangesAsync();
            }
            return true;
        }
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = await Repository.GetAsync(x => x.CreatedBy == App.Auth.Email).ToListAsync();
            return JsonConvert.DeserializeObject<List<CategoryDTO>>(JsonConvert.SerializeObject(categories));
        }
    }
}
