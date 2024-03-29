using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shoping.Presentation.View;
using Shoping.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        private Configuration AppConfig { get; set; }
        public Login()
        {
            InitializeComponent();
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "AppConfig.config";
            AppConfig = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LoginViewModel = new LoginViewModel(App.iUserBusiness);
            this.DataContext = LoginViewModel;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Email.Text = GetConfiguration("UserName");
            var pw = GetConfiguration("Password");
            if (!string.IsNullOrEmpty(pw))
            {
                var pwBytes = Convert.FromBase64String(pw);
                var entropy = GetConfiguration("Entropy");
                var entropyBytes = Convert.FromBase64String(entropy);
                var unecryptedPassword = ProtectedData.Unprotect(pwBytes, entropyBytes, DataProtectionScope.CurrentUser);
                Password.Password = Encoding.UTF8.GetString(unecryptedPassword);
                ckbSave.IsChecked = true;
            }
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
            var pwBytes = Encoding.UTF8.GetBytes(password);
            var md5PWBytes = MD5.HashData(pwBytes);
            var md5PW = Convert.ToHexString(md5PWBytes);

            var isSuccess = await LoginViewModel.Login(email, md5PW);
            if (isSuccess)
            {
                var mainView = new Main();
                if (ckbSave.IsChecked == true)
                {
                    var entropyBytes = new byte[20];
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(entropyBytes);
                    }
                    var encryptedPWBytes = ProtectedData.Protect(pwBytes, entropyBytes, DataProtectionScope.CurrentUser);
                    var encryptedPW = Convert.ToBase64String(encryptedPWBytes);
                    SetConfiguration("UserName", email);
                    SetConfiguration("Password", encryptedPW);
                    SetConfiguration("Entropy", Convert.ToBase64String(entropyBytes));
                }
                MessageBox.Show("Đăng nhập thành công");
                //mainView.Closed += ReturnToLogin;
                this.Close();
                mainView.Show();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
            }
        }

        private void ReturnToLogin(object sender, EventArgs e)
        {
            this.Show();
        }

        private void SetConfiguration(string key, string value)
        {
            var settings = AppConfig.AppSettings.Settings;
            if (settings[key] != null)
            {
                settings[key].Value = value;
            }
            else
            {
                settings.Add(key, value);
            }
            AppConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(AppConfig.AppSettings.SectionInformation.Name);
        }

        private string GetConfiguration(string key)
        {
            if (AppConfig.AppSettings.Settings[key] != null)
            {
                return AppConfig.AppSettings.Settings[key].Value;
            }
            return "";
        }


    }
}
