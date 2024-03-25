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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<ProductDTO> productDTOs = await MainViewModel.GetAllProducts();

            _products = new ObservableCollection<ProductDTO>();
            foreach (var productDTO in productDTOs)
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
        private async void loadData()
        {
            List<ProductDTO> productDTOs = await MainViewModel.GetAllProducts();

            _products = new ObservableCollection<ProductDTO>();
            foreach (var productDTO in productDTOs)
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

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddProduct();
            if(screen.ShowDialog() == true)
            {
                ProductDTO temp = screen.newPhone;
                await MainViewModel.AddUpdateProduct(temp);
                loadData();
                MessageBox.Show("Added successfully!");
            }
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = productsListView.SelectedItem as ProductDTO;
            await MainViewModel.DeleteProduct(selected);
            loadData();
            MessageBox.Show("Deleted successfully!");
        }

        private async void editButton_Click(object sender, RoutedEventArgs e)
        {
            ProductDTO oldData = null;
            var selected = productsListView.SelectedItem as ProductDTO;
            oldData = (ProductDTO)selected.Clone();
            var screen = new EditProduct(selected);
            if(screen.ShowDialog() == true)
            {
                selected.Name = screen.newEditPhone.Name;
                selected.Price = screen.newEditPhone.Price;
                selected.Image = screen.newEditPhone.Image;
                await MainViewModel.AddUpdateProduct(selected);
                loadData();
                MessageBox.Show("Edited successfully");
            }
            else
            {
                selected.Name = oldData.Name;
                selected.Price = oldData.Price;
                selected.Image = oldData.Image;
                loadData();
            }
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var search = searchTextBox.Text;
            if(search == "")
            {
                loadData();
            }
            else
            {
                List<ProductDTO> productDTOs = await MainViewModel.SearchProduct(search);

                _products = new ObservableCollection<ProductDTO>();
                foreach (var productDTO in productDTOs)
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
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StatisticButton_Click(object sender, RoutedEventArgs e)
        {
            Statistic statistic = new();
            statistic.Show();
        }
    }
}
