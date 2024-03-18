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
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using Shoping.Presentation.Control;

namespace Shoping.Presentation.View
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        public ProductDTO newEditPhone { get; set; }
        public EditProduct(ProductDTO temp)
        {
            InitializeComponent();
            newEditPhone = (ProductDTO)temp.Clone();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = newEditPhone;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
