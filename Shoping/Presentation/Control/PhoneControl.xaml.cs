using Microsoft.Win32;
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
    /// Interaction logic for PhoneControl.xaml
    /// </summary>
    public partial class PhoneControl : UserControl
    {
        private static string fullPath;
        public PhoneControl()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png"
            };
            if (file.ShowDialog() == true)
            {
                fullPath = file.FileName;
            }

            BitmapImage picture = new();
            picture.BeginInit();
            picture.UriSource = new Uri(fullPath);
            picture.EndInit();
            phoneImage.Source = picture;
        }
    }
}
