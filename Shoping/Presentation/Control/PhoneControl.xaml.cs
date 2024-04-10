using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Shoping.Presentation.Control
{
    /// <summary>
    /// Interaction logic for PhoneControl.xaml
    /// </summary>
    public partial class PhoneControl : UserControl
    {
        public string fullPath { get; set; }
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
                var fileBytes = File.ReadAllBytes(fullPath);
                BitmapImage picture = new();
                picture.BeginInit();
                //picture.UriSource = new Uri(fullPath);
                picture.StreamSource = new MemoryStream(fileBytes);
                picture.EndInit();
                phoneImage.Source = picture;
            }
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
