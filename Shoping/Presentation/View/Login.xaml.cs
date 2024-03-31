using Shoping.Presentation.View;
using Shoping.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shoping.Presentation
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// qui
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
                this.Close();
                mainView.Show();
            }
        }

        private void ReturnToLogin(object sender, EventArgs e)
        {
            this.Show();
        }

        /*public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Server { get; set; }
        public string Database { get; set; }
        public string Entropy { get; set; }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            AppConfig.Password = passwordBox.Password;
            AppConfig.Username = Username;

            var connectionString = AppConfig.ConnectionString();

            progressBar.IsIndeterminate = true;
            var (success, message, connection) = await Task.Run(() => {
                var __connection = new SqlConnection(connectionString);
                bool success = true;
                string message = "";
                try
                {
                    System.Threading.Thread.Sleep(1000);
                    __connection.Open();
                }
                catch (Exception ex)
                {
                    success = false;
                    message = ex.Message;
                }

                // Test khi chạy quá nhanh

                return new Tuple<bool, string, SqlConnection>(success, message, __connection);
            });
            progressBar.IsIndeterminate = false;
            if (success)
            {
                MessageBox.Show("Login successfully");

                if (rememberPassCheckBox.IsChecked == true)
                {
                    var passwordInBytes = Encoding.UTF8.GetBytes(passwordBox.Password);
                    var entropy = new byte[20];

                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(entropy);
                    }
                    var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
                    AppConfig.PasswordIn64 = Convert.ToBase64String(cypherText);
                    AppConfig.Entropy = Convert.ToBase64String(entropy);
                    AppConfig.Save();
                }

                var screen = new Main();
                screen.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Cannot login. Reason: {message}");
            }
        }

        private void serverSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Config();
            if (screen.ShowDialog() == true)
            {
                AppConfig.Reload();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppConfig.Reload();

            Username = AppConfig.Username;
            Password = AppConfig.Password;

            DataContext = this;

            passwordBox.Password = AppConfig.Password;
        }*/
    } 
}
