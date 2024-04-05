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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        public CategoryDTO newCategory = new CategoryDTO();
        public AddCategory()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(categoryControl.nameTextBox.Text.Trim()))
            {
                MessageBox.Show("Please fill all the information!");
            }
            else
            {
                newCategory.RecID = new Guid();
                newCategory.CategoryID = new Guid();
                newCategory.Name = categoryControl.nameTextBox.Text;
                DialogResult = true;
            }
        }
    }
}
