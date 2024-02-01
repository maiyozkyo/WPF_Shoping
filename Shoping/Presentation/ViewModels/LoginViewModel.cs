using Newtonsoft.Json;
using PropertyChanged;
using Shoping.Business.UserServices;
using Shoping.Data_Access.DTOs;
using System;


namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]                                                                                                                  
    public class LoginViewModel
    {
        public IUserServices UserServices;
        public LoginViewModel(IUserServices userServices)
        {
            UserServices = userServices;
        }

        public async Task<bool> Login(string email, string password)
        {
            var user = await UserServices.LoginAsync(email, password);
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
