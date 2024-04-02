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
        public EditProduct(ProductDTO temp, List<CategoryDTO> categories)
        {
            InitializeComponent();
            newEditPhone = (ProductDTO)temp.Clone();
            foreach (CategoryDTO category in categories)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = category.Name;
                item.Tag = category.RecID;

                phoneControl.categoryComboBox.Items.Add(item);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = newEditPhone;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newEditPhone.Name = phoneControl.nameTextBox.Text;
            newEditPhone.Price = decimal.Parse(phoneControl.priceTextBox.Text);
            newEditPhone.PurchasePrice = decimal.Parse(phoneControl.purchasePriceTextBox.Text);
            newEditPhone.CatID = Guid.Parse(((ComboBoxItem)phoneControl.categoryComboBox.SelectedItem).Tag.ToString());
            newEditPhone.Quantity = int.Parse(phoneControl.quantityTextBox.Text);
            newEditPhone.Image = phoneControl.phoneImage.Source.ToString();
            DialogResult = true;
        }
    }
}
