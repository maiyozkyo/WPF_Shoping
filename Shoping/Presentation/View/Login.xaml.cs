using Shoping.Presentation.View;
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
            this.DataContext = LoginViewModel;
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            var registerWindow = new SignUp();
            registerWindow.Closed += ReturnToLogin;
            this.Hide();
            registerWindow.Show();
        }

        

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            var email = Email.Text;
            var password = Password.Password;
            var isSuccess = await LoginViewModel.Login(email, password);
            if (isSuccess)
            {
                var mainView = new Main();
                mainView.Closed += ReturnToLogin;
                this.Hide();
                mainView.Show();
            }
        }

        private void ReturnToLogin(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
