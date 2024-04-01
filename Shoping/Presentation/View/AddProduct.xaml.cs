using System;
using System.Collections.Generic;
using System.IO;
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
        public AddProduct(List<CategoryDTO> categories)
        {
            InitializeComponent();
            foreach(CategoryDTO category in categories)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = category.Name;
                item.Tag = category.RecID;

                phoneControl.categoryComboBox.Items.Add(item);
            }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newPhone.RecID = Guid.NewGuid();
            newPhone.ProductID = Guid.NewGuid();
            newPhone.Name = phoneControl.nameTextBox.Text;
            newPhone.Price = decimal.Parse(phoneControl.priceTextBox.Text);
            newPhone.PurchasePrice = decimal.Parse(phoneControl.purchasePriceTextBox.Text);
            newPhone.CatID = Guid.Parse(((ComboBoxItem)phoneControl.categoryComboBox.SelectedItem).Tag.ToString());
            newPhone.Quantity = int.Parse(phoneControl.quantityTextBox.Text);

            string directory = phoneControl.fullPath;
            var fileBytes = File.ReadAllBytes(directory);
            newPhone.Image = Convert.ToBase64String(fileBytes);

            DialogResult = true;
        }
    }
}
