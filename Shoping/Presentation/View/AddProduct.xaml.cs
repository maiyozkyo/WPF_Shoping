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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public ProductDTO newPhone = new ProductDTO();
        public AddProduct()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newPhone.RecID = Guid.NewGuid();
            newPhone.ProductID = Guid.NewGuid();
            newPhone.Name = phoneControl.nameTextBox.Text;
            newPhone.Price = int.Parse(phoneControl.priceTextBox.Text);
            String directory = phoneControl.phoneImage.Source.ToString();
            // Split the directory string by "/"
            string[] parts = directory.Split('/');

            // Get the last part which contains the filename
            // Remove the quotes and any extra characters
            newPhone.Image = "Images/" + parts[parts.Length - 1].Trim('\"');
            DialogResult = true;
        }
    }
}
