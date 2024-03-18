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
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.AddUpdateOrder();
        }*/

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.AddUpdateProduct(null);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = productsListView.SelectedItem as ProductDTO;
            MainViewModel.DeleteProduct(selected);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = productsListView.SelectedItem as ProductDTO;
            MainViewModel.DeleteProduct(selected);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var search = searchTextBox.Text;
            MainViewModel.SearchProduct(search);
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
    }
}
