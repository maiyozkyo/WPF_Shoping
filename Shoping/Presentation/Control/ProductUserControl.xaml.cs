﻿using Microsoft.Win32;
using Shoping.Business.Helper;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using Shoping.Presentation.View;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
        public double priceFromFilter = 0;
        public double priceToFilter = double.MaxValue;
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
            pagingProducts = await MainViewModel.GetFilterProducts(searchFilter, categoryFilter, priceFromFilter, priceToFilter, page, _paging.itemsPerPage);

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
                pagesComboBox.SelectedIndex = 0;
                MessageBox.Show("Added successfully!");
            }
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = productsListView.SelectedItem as ProductDTO;
            if(selected != null)
            {
                await MainViewModel.DeleteProduct(selected);
                loadData(1);
                pagesComboBox.SelectedIndex = 0;
                MessageBox.Show("Deleted successfully!");
            }
            else
            {
                MessageBox.Show("You have to choose an item to delete!");
            }
        }

        private async void editButton_Click(object sender, RoutedEventArgs e)
        {
            var categoryList = await categoryViewModel.GetAllCategories();
            ProductDTO oldData = null;
            var selected = productsListView.SelectedItem as ProductDTO;
            if(selected != null)
            {
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
            else
            {
                MessageBox.Show("You have to choose an item to edit!");
            }
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            searchFilter = searchTextBox.Text;
            loadData(1);
            pagesComboBox.SelectedIndex = 0;
            MessageBox.Show("Search filter successfully!");
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
            if ((ComboBoxItem)categoryComboBox.SelectedItem != null)
            {
                categoryFilter = Guid.Parse(((ComboBoxItem)categoryComboBox.SelectedItem).Tag.ToString());
            }
            //loadData(1);
            pagesComboBox.SelectedIndex = 0;
        }

        private async void excelButton_Click(object sender, RoutedEventArgs e)
        {
            string fullPath = "";
            OpenFileDialog file = new()
            {
                Filter = "Excel Files|*.xlsx"
            };
            
            if (file.ShowDialog() == true)
            {
                progressBar.IsIndeterminate = true;
                fullPath = file.FileName;
                var fileBytes = File.ReadAllBytes(fullPath);
                await categoryViewModel.DeleteAllCategories();
                await MainViewModel.DeleteAllProducts();
                using (var mem = new MemoryStream(fileBytes))
                {
                    Dictionary<int, Guid> catID = new Dictionary<int, Guid>();

                    var listCategories = ExcelHelper.ReadAsList<CategoryDTO>(mem, 0);
                    for (int i = 0; i < listCategories.Count(); i++)
                    {
                        await categoryViewModel.AddUpdateCategory(listCategories[i]);
                        catID[i] = await categoryViewModel.GetCategoryID(listCategories[i].Name);
                    }
                    var listProducts = ExcelHelper.ReadAsList<ProductExcelDTO>(mem, 1);
                    foreach (var product in listProducts)
                    {
                        for (int i = 0; i < catID.Count(); i++)
                        {
                            if (product.CatID == (i + 1).ToString())
                            {
                                var tempProduct = new ProductDTO
                                {
                                    Name = product.Name,
                                    Price = product.Price,
                                    PurchasePrice = product.PurchasePrice,
                                    Quantity = product.Quantity,
                                    CatID = catID[i],
                                    Image = product.Image
                                };
                                await MainViewModel.AddUpdateProduct(tempProduct);
                            }
                        }
                    }
                }
                loadData(1);
                pagesComboBox.SelectedIndex = 0;
                progressBar.IsIndeterminate = false;
                MessageBox.Show("Loaded successfully!");
            }
        }

        private void priceSortButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(priceSortFromTextBox.Text) && !string.IsNullOrEmpty(priceSortToTextBox.Text))
            {
                if (!double.TryParse(priceSortFromTextBox.Text, out priceFromFilter) || !double.TryParse(priceSortToTextBox.Text, out priceToFilter))
                {
                    MessageBox.Show("Only numbers are allowed!");
                }
                else
                {
                    priceFromFilter = double.Parse(priceSortFromTextBox.Text);
                    priceToFilter = double.Parse(priceSortToTextBox.Text);
                    if (priceFromFilter > priceToFilter)
                    {
                        MessageBox.Show("Error! Price from cannot larger than price to");
                    }
                    else
                    {
                        loadData(1);
                        pagesComboBox.SelectedIndex = 0;
                        MessageBox.Show("Price filter successfully!");
                    }
                }
            }
            else if((string.IsNullOrEmpty(priceSortFromTextBox.Text) && !string.IsNullOrEmpty(priceSortToTextBox.Text)) || (!string.IsNullOrEmpty(priceSortFromTextBox.Text) && string.IsNullOrEmpty(priceSortToTextBox.Text)))
            {
                MessageBox.Show("Please fill in both prices for sorting!");
            }
            else
            {
                priceFromFilter = 0;
                priceToFilter = double.MaxValue;
                loadData(1);
                pagesComboBox.SelectedIndex = 0;
                MessageBox.Show("All datas have been loaded!");
            }
            //priceFromFilter = !string.IsNullOrEmpty(priceSortFromTextBox.Text) ? double.Parse(priceSortFromTextBox.Text) : 0;
            //priceToFilter = !string.IsNullOrEmpty(priceSortToTextBox.Text) ? double.Parse(priceSortToTextBox.Text) : int.MaxValue;
        }
    }
}
