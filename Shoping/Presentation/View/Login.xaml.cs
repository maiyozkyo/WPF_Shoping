using Shoping.Presentation.ViewModels;
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

namespace Shoping.Presentation
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public LoginViewModel LoginViewModel { get; set; }
        public Login()
        {
            InitializeComponent();
            LoginViewModel = new LoginViewModel(App.iUserBusiness);

        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            var registerWindow = new SignUp();
            registerWindow.Closed += RegisterClose;
            this.Hide();
            registerWindow.Show();
        }

        private void RegisterClose(object sender, EventArgs e)
        {
            this.Show();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            var email = UserName.Text;
            var password = Password.Password;
            var user = LoginViewModel.UserBusiness.GetUserAsync(email, password);
            if (user != null)
            {
                this.Close();
            }
        }
    }
}
