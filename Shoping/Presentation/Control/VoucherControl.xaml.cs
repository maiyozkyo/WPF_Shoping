using Shoping.Presentation.ViewModels;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shoping.Presentation.Control
{
    /// <summary>
    /// Interaction logic for VoucherControl.xaml
    /// </summary>
    public partial class VoucherControl : UserControl
    {
        public VoucherViewModel VoucherViewModel { get; set; }
        public VoucherControl()
        {
            InitializeComponent();
            VoucherViewModel = new VoucherViewModel(App.iVoucherBusiness);
            DataContext = VoucherViewModel;
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await GetDataVouchers();
        }
        private void BtnGenCode_Click(object sender, RoutedEventArgs e)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int length = 12;
            StringBuilder sb = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }
            VoucherViewModel.Code = sb.ToString();
        }

        private void CheckIsNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private async Task GetDataVouchers()
        {
            await VoucherViewModel.GetVouchers();
            ListVouchers.ItemsSource = VoucherViewModel.LstVouchers;
        }

        private async void BtnAddVoucher_Click(object sender, RoutedEventArgs e)
        {
            await VoucherViewModel.AddUpdateVoucher();
            await GetDataVouchers();
            dateVoucher.SelectedDate = DateTime.Now;
        }
    }
}
