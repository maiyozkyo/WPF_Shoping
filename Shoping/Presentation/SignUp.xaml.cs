using Microsoft.Extensions.Configuration;
using Shoping.Business;
using Shoping.Data_Access.DB.UnitOfWork;
using Shoping.Data_Access.Models;
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
        private IUserBusiness _userBusiness;
        public SignUp()
        {
            InitializeComponent();
        }

        private void AddUserAsync(object sender, RoutedEventArgs e)
        {
            if (Password.Password != ConfirmPassword.Password)
            {
                return;
            }

            var user = new User();
            user.Email = Email.Text;
            user.Password = Password.Password;
            var addedUser = _userBusiness.AddUpdateUserAsync(user).Result;
        }
    }
}
