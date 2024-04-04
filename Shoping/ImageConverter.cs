using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Shoping
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                BitmapImage picture = new();
                picture.BeginInit();
                picture.StreamSource = new MemoryStream(System.Convert.FromBase64String((string)value));
                picture.EndInit();
                /*string filePath = (string)value;
                if (Uri.TryCreate(filePath, UriKind.Absolute, out Uri uri) && uri.IsFile)
                {
                    return uri.LocalPath;
                }*/
                return picture;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
