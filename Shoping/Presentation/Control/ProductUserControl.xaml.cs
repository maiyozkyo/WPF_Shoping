using Shoping.Business.Helper;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using Shoping.Presentation.View;
using Shoping.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Shoping.Presentation.Control
{
    /// <summary>
    /// Interaction logic for ProductUserControl.xaml
    /// </summary>
    public partial class ProductUserControl : UserControl
    {
        public MainViewModel MainViewModel { get; set; }
        public CategoryViewModel categoryViewModel { get; set; }
        public ObservableCollection<ProductDTO> _products { get; set; }
        public List<CategoryDTO> _categories { get; set; }
        public PageData<ProductDTO> pagingProducts { get; set; }
        public string searchFilter = "";
        public Guid categoryFilter = Guid.Empty;
        PagingInfo _paging;
        public SettingUserControl setting = new SettingUserControl();
        public int _itemsPerPage { get; set; }
        class PagingInfo
        {
            public int currentPage { get; set; }
            public int totalPage { get; set; }
            public int itemsPerPage { get; set; }
    }
        public ProductUserControl()
        {
            InitializeComponent();
            _paging = new PagingInfo();
            MainViewModel = new MainViewModel(App.iProductBusiness);
            categoryViewModel = new CategoryViewModel(App.iCategoryBusiness);
            DataContext = MainViewModel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Loading products
            //_paging = new PagingInfo();
            //_paging.itemsPerPage = ItemsPerPage.itemsPerPage;
            //pagingProducts = await MainViewModel.Paging(1, _paging.itemsPerPage);

            //_products = new ObservableCollection<ProductDTO>(pagingProducts.Data);
            //foreach (var productDTO in pagingProducts.Data)
            //{
            //    _products.Add(new ProductDTO
            //    {
            //        RecID = productDTO.RecID,
            //        ProductID = productDTO.ProductID,
            //        Name = productDTO.Name,
            //        Price = productDTO.Price,
            //        PurchasePrice = productDTO.PurchasePrice,
            //        CatID = productDTO.CatID,
            //        Quantity = productDTO.Quantity,
            //        Image = productDTO.Image
            //    });
            //}
            //productsListView.ItemsSource = _products;
            //DataContext = MainViewModel;

            // Implement paging
            //_paging.currentPage = 1;
            //_paging.totalPage = (pagingProducts.Total % pagingProducts.Data.Count() == 0) ? pagingProducts.Total / pagingProducts.Data.Count() : pagingProducts.Total / pagingProducts.Data.Count() + 1;


            //var infos = new ObservableCollection<object>();
            //for (int i = 1; i <= _paging.totalPage; i++)
            //{
            //    infos.Add(new
            //    {
            //        Index = i,
            //        Total = _paging.totalPage
            //    });
            //}

            var infos = new List<object>();
            infos.Add(new
            {
                Index = 1,
                Total = 1
            });

            pagesComboBox.ItemsSource = new ObservableCollection<object>(infos);
            pagesComboBox.SelectedIndex = 0;

            // Handling category filter
            categoryComboBox.Items.Clear();
            _categories = await categoryViewModel.GetAllCategories();
            ComboBoxItem item = new ComboBoxItem();
            item.Content = "All";
            item.Tag = Guid.Empty;

            categoryComboBox.Items.Add(item);
            foreach (CategoryDTO category in _categories)
            {
                item = new ComboBoxItem();
                item.Content = category.Name;
                item.Tag = category.RecID;

                categoryComboBox.Items.Add(item);
            }
            categoryComboBox.SelectedIndex = 0;
        }


        private async void loadData(int page)
        {
            _paging.itemsPerPage = ItemsPerPage.itemsPerPage;
            pagingProducts = await MainViewModel.GetFilterProducts(searchFilter, categoryFilter, page, _paging.itemsPerPage);

            _products = new ObservableCollection<ProductDTO>(pagingProducts.Data);
            productsListView.ItemsSource = _products;
            DataContext = MainViewModel;

            _paging.currentPage = page;
            _paging.totalPage = (pagingProducts.Total % _paging.itemsPerPage == 0) ? pagingProducts.Total / _paging.itemsPerPage : pagingProducts.Total / _paging.itemsPerPage + 1;


            var infos = new ObservableCollection<object>();
            for (int i = 1; i <= _paging.totalPage; i++)
            {
                infos.Add(new
                {
                    Index = i,
                    Total = _paging.totalPage
                });
            }

            pagesComboBox.ItemsSource = infos;
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            var categoryList = await categoryViewModel.GetAllCategories();
            var screen = new AddProduct(categoryList);
            if (screen.ShowDialog() == true)
            {
                ProductDTO temp = screen.newPhone;
                await MainViewModel.AddUpdateProduct(temp);
                loadData(1);
                MessageBox.Show("Added successfully!");
            }
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = productsListView.SelectedItem as ProductDTO;
            await MainViewModel.DeleteProduct(selected);
            loadData(1);
            MessageBox.Show("Deleted successfully!");
        }

        private async void editButton_Click(object sender, RoutedEventArgs e)
        {
            var categoryList = await categoryViewModel.GetAllCategories();
            ProductDTO oldData = null;
            var selected = productsListView.SelectedItem as ProductDTO;
            oldData = (ProductDTO)selected.Clone();
            var screen = new EditProduct(selected, categoryList);
            if (screen.ShowDialog() == true)
            {
                selected.Name = screen.newEditPhone.Name;
                selected.Price = screen.newEditPhone.Price;
                selected.PurchasePrice = screen.newEditPhone.PurchasePrice;
                selected.CatID = screen.newEditPhone.CatID;
                selected.Quantity = screen.newEditPhone.Quantity;
                selected.Image = screen.newEditPhone.Image;
                await MainViewModel.AddUpdateProduct(selected);
                loadData(_paging.currentPage);
                MessageBox.Show("Edited successfully");
            }
            else
            {
                selected.Name = oldData.Name;
                selected.Price = oldData.Price;
                selected.Image = oldData.Image;
                loadData(_paging.currentPage);
            }
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            searchFilter = searchTextBox.Text;
            loadData(1);
            pagesComboBox.SelectedIndex = 0;
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_paging.currentPage > 1)
            {
                _paging.currentPage--;
                pagesComboBox.SelectedIndex--;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_paging.currentPage < _paging.totalPage)
            {
                _paging.currentPage++;
                pagesComboBox.SelectedIndex++;
            }
        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = (pagesComboBox.SelectedIndex >= 0 ? pagesComboBox.SelectedIndex : 0);
            _paging.currentPage = i + 1;
            loadData(_paging.currentPage);
        }

        private async void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((ComboBoxItem)categoryComboBox.SelectedItem != null)
            {
                categoryFilter = Guid.Parse(((ComboBoxItem)categoryComboBox.SelectedItem).Tag.ToString());
            }
            loadData(1);
            pagesComboBox.SelectedIndex = 0;
        }

        private void excelButton_Click(object sender, RoutedEventArgs e)
        {
            var fileBytes = File.ReadAllBytes("D:\\Nam 3\\HK2\\Windows\\Project 1\\MyShop\\Shoping\\Book1.xlsx");
            using (var mem = new MemoryStream(fileBytes))
            {
                var lstData = ExcelHelper.ReadAsList<ProductDTO>(mem);

            }
        }
    }
}
