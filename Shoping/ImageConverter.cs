﻿using System.Globalization;
using System.Windows.Data;

namespace Shoping
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if ((string)value != null)
            //{
            //    BitmapImage picture = new();
            //    picture.BeginInit();
            //    picture.StreamSource = new MemoryStream(System.Convert.FromBase64String((string)value));
            //    picture.EndInit();
            //    /*string filePath = (string)value;
            //    if (Uri.TryCreate(filePath, UriKind.Absolute, out Uri uri) && uri.IsFile)
            //    {
            //        return uri.LocalPath;
            //    }*/
            //    return picture;
            //}
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
