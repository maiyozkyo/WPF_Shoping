using Microsoft.Extensions.Configuration;
using Shoping.Business;
using Shoping.Data_Access.DB.UnitOfWork;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
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
                return;
            }

            var user = new UserDTO();
            user.Email = Email.Text;
            user.Password = Password.Password;
            var addedUser = await SignUpViewModel.UserServices.AddUpdateUserAsync(user);
            if (addedUser != null)
            {
                this.Close();
            }
        }
    }
}
