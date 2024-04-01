using Shoping.Data_Access.Models;
using Shoping.Presentation.ViewModels;
using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Shoping.Presentation.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public MainViewModel MainViewModel { get; set; }
        class PagingInfo
        {
            public int currentPage { get; set; }
            public int totalPage { get; set; }
        }
        
        public Main()
        {
            InitializeComponent();
            MainViewModel = new MainViewModel(App.iProductBusiness);
            DataContext = MainViewModel;
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.AddUpdateOrder();
        }*/
        public ObservableCollection<ProductDTO> _products { get; set; }
        PagingInfo _paging;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PageData<ProductDTO> paging = await MainViewModel.Paging(1, 4);

            _products = new ObservableCollection<ProductDTO>();
            foreach (var productDTO in paging.Data)
            {
                _products.Add(new ProductDTO 
                {   
                    RecID = productDTO.RecID,
                    ProductID = productDTO.ProductID,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Image = productDTO.Image
                });
            }
            productsListView.ItemsSource = _products;
            DataContext = MainViewModel;

            _paging = new PagingInfo();

            _paging.currentPage = 1;
            _paging.totalPage = (paging.Total % paging.Data.Count() == 0) ? paging.Total / paging.Data.Count() : paging.Total / paging.Data.Count() + 1;

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
            pagesComboBox.SelectedIndex = 0;
        }
        private async void loadData(int page)
        {
            PageData<ProductDTO> paging = await MainViewModel.Paging(page, 4);

            _products = new ObservableCollection<ProductDTO>();
            foreach (var productDTO in paging.Data)
            {
                _products.Add(new ProductDTO
                {
                    RecID = productDTO.RecID,
                    ProductID = productDTO.ProductID,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Image = productDTO.Image
                });
            }

            productsListView.ItemsSource = _products;
            DataContext = MainViewModel;
        }

        private async void loadDataSearch(String search, int page)
        {
            /*PageData<ProductDTO> paging = await MainViewModel.SearchProduct(search, page, 4);

            _products = new ObservableCollection<ProductDTO>();
            foreach (var productDTO in paging.Data)
            {
                _products.Add(new ProductDTO
                {
                    RecID = productDTO.RecID,
                    ProductID = productDTO.ProductID,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Image = productDTO.Image
                });
            }

            productsListView.ItemsSource = _products;
            DataContext = MainViewModel;*/
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            /*var screen = new AddProduct();
            if(screen.ShowDialog() == true)
            {
                ProductDTO temp = screen.newPhone;
                await MainViewModel.AddUpdateProduct(temp);
                loadData(_paging.currentPage);
                MessageBox.Show("Added successfully!");
            }*/
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = productsListView.SelectedItem as ProductDTO;
            await MainViewModel.DeleteProduct(selected);
            loadData(_paging.currentPage);
            MessageBox.Show("Deleted successfully!");
        }

        private async void editButton_Click(object sender, RoutedEventArgs e)
        {
            /*ProductDTO oldData = null;
            var selected = productsListView.SelectedItem as ProductDTO;
            oldData = (ProductDTO)selected.Clone();
            var screen = new EditProduct(selected);
            if(screen.ShowDialog() == true)
            {
                selected.Name = screen.newEditPhone.Name;
                selected.Price = screen.newEditPhone.Price;
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
            }*/
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            /*var search = searchTextBox.Text;
           
            PageData<ProductDTO> paging = await MainViewModel.SearchProduct(search, 1, 4);

            _products = new ObservableCollection<ProductDTO>();
            foreach (var productDTO in paging.Data)
            {
                _products.Add(new ProductDTO
                {
                    RecID = productDTO.RecID,
                    ProductID = productDTO.ProductID,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Image = productDTO.Image
                });
            }
            productsListView.ItemsSource = _products;
            DataContext = MainViewModel;

            _paging = new PagingInfo();

            _paging.currentPage = 1;
            _paging.totalPage = (paging.Total % paging.Data.Count() == 0) ? paging.Total / paging.Data.Count() : paging.Total / paging.Data.Count() + 1;

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
            pagesComboBox.SelectedIndex = 0;
*/
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if(_paging.currentPage > 1)
            {
                _paging.currentPage--;
                pagesComboBox.SelectedIndex--;
                loadData(_paging.currentPage);
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_paging.currentPage < _paging.totalPage)
            {
                _paging.currentPage++;
                pagesComboBox.SelectedIndex++;
                loadData(_paging.currentPage);
            }
        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var search = searchTextBox.Text;
            int i = (pagesComboBox.SelectedIndex >= 0 ? pagesComboBox.SelectedIndex : 0);
            _paging.currentPage = i + 1;
            loadDataSearch(search, _paging.currentPage);
        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            Statistic statistic = new();
            statistic.Show();
        }
    }
}
