﻿using PropertyChanged;
using Shoping.Business.CategoryServices;
using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CategoryViewModel
    {
        public ICategoryBusiness CategoryBusiness;
        public List<ProductDTO> products { get; set; }
        public CategoryViewModel(ICategoryBusiness categoryBusiness)
        {
            CategoryBusiness = categoryBusiness;
        }

        public async Task<Guid> AddUpdateCategory(CategoryDTO categoryDTO)
        {
            var result = await CategoryBusiness.AddUpdateCategoryAsync(categoryDTO);
            return result;
        }

        public async Task<bool> DeleteCategory(CategoryDTO categoryDTO)
        {
            var result = await CategoryBusiness.DeleteCategoryAsync(categoryDTO.RecID);
            return result;
        }
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var result = await CategoryBusiness.GetAllCategories();
            return result;
        }
        public async Task<bool> DeleteAllCategories()
        {
            var result = await CategoryBusiness.DeleteAllCategories();
            return result;
        }
        public async Task<Guid> GetCategoryID(string name)
        {
            var result = await CategoryBusiness.GetCategoryID(name);
            return result;
        }
    }
}
