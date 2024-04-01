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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shoping.Presentation.Control
{
    /// <summary>
    /// Interaction logic for SettingUserControl.xaml
    /// </summary>
    public partial class SettingUserControl : UserControl
    {
        public delegate void NewItemsPerPage(int size);
        public event NewItemsPerPage NewItemsPerPageUpdating;

        //public int ItemsPerPage { get; set; }

        public SettingUserControl()
        {
            InitializeComponent();
            comboBox.Items.Add(4);
            comboBox.Items.Add(8);
            comboBox.Items.Add(12);
            comboBox.SelectedIndex = 0;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = comboBox.SelectedIndex;
            int pageSize = selectedIndex != -1 ?(int)comboBox.Items[selectedIndex] : 1;
            NewItemsPerPageUpdating?.Invoke(pageSize);
            ItemsPerPage.itemsPerPage = pageSize;
            
        }
    }
}
