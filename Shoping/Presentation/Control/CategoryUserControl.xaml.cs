using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using Shoping.Presentation.View;
using Shoping.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shoping.Presentation.Control
{
    /// <summary>
    /// Interaction logic for CategoryUserControl.xaml
    /// </summary>
    public partial class CategoryUserControl : UserControl
    {
        public CategoryViewModel CategoryViewModel { get; set; }
        public ObservableCollection<CategoryDTO> _categories { get; set; }
        public MainViewModel MainViewModel { get; set; }
        public CategoryUserControl()
        {
            InitializeComponent();
            CategoryViewModel = new CategoryViewModel(App.iCategoryBusiness);
            MainViewModel = new MainViewModel(App.iProductBusiness);
            DataContext = CategoryViewModel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<CategoryDTO> categories = await CategoryViewModel.GetAllCategories();

            _categories = new ObservableCollection<CategoryDTO>();
            foreach (var categoryDTO in categories)
            {
                _categories.Add(new CategoryDTO
                {
                    RecID = categoryDTO.RecID,
                    CategoryID = categoryDTO.CategoryID,
                    Name = categoryDTO.Name
                });
            }
            categoriesListView.ItemsSource = _categories;
            DataContext = CategoryViewModel;
        }
        private async void loadData()
        {
            List<CategoryDTO> categories = await CategoryViewModel.GetAllCategories();

            _categories = new ObservableCollection<CategoryDTO>();
            foreach (var categoryDTO in categories)
            {
                _categories.Add(new CategoryDTO
                {
                    RecID = categoryDTO.RecID,
                    CategoryID = categoryDTO.CategoryID,
                    Name = categoryDTO.Name
                });
            }
            categoriesListView.ItemsSource = _categories;
            DataContext = CategoryViewModel;
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddCategory();
            if (screen.ShowDialog() == true)
            {
                CategoryDTO temp = screen.newCategory;
                await CategoryViewModel.AddUpdateCategory(temp);
                loadData();
                MessageBox.Show("Added successfully!");
            }
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = categoriesListView.SelectedItem as CategoryDTO;
            bool checkProduct = await MainViewModel.CheckProductCategory(selected.RecID);
            if (checkProduct)
            {
                MessageBox.Show("Cannot delete category. Reason: There are still products with this category!");
            }
            else
            {
                await CategoryViewModel.DeleteCategory(selected);
                loadData();
                MessageBox.Show("Deleted successfully!");
            }
        }

        private async void editButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryDTO oldData = null;
            var selected = categoriesListView.SelectedItem as CategoryDTO;
            oldData = (CategoryDTO)selected.Clone();
            var screen = new EditCategory(selected);
            if (screen.ShowDialog() == true)
            {
                selected.Name = screen.newEditCategory.Name;
                await CategoryViewModel.AddUpdateCategory(selected);
                loadData();
                MessageBox.Show("Edited successfully");
            }
            else
            {
                selected.Name = oldData.Name;
                loadData();
            }
        }
    }
}
