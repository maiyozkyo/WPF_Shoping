using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Shoping.Presentation
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUpViewModel SignUpViewModel { get; set; }
        public SignUp()
        {
            InitializeComponent();
            SignUpViewModel = new SignUpViewModel(App.iUserBusiness);
        }

        private async void AddUserAsync(object sender, RoutedEventArgs e)
        {
            if (Password.Password != ConfirmPassword.Password)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp");
                return;
            }

            var user = new UserDTO();
            user.Email = Email.Text;
            var pwBytes = Encoding.UTF8.GetBytes(Password.Password);
            var md5PWBytes = MD5.HashData(pwBytes);
            var md5PW = Convert.ToHexString(md5PWBytes);
            user.Password = md5PW;
            var addedUser = await SignUpViewModel.UserServices.AddUpdateUserAsync(user);
            if (addedUser != null)
            {
                MessageBox.Show("Đăng ký thành công");
                var loginView = new Login();
                this.Close();
                loginView.Show();
            }
            else
            {
                MessageBox.Show("Đã tồn tại tài khoản này, vui lòng sử dụng tài khoản khác");
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            var loginView = new Login();
            this.Close();
            loginView.Show();
        }
    }
}
