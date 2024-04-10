using System.Globalization;
using System.Windows.Data;

namespace Shoping
{
    class PaymentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool paymentStatus = (bool)value;
                return paymentStatus ? "Đã thanh toán" : "Chưa thanh toán";
            }
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
