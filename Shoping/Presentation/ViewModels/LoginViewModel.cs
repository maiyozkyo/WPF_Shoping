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
        public IUserBusiness UserServices;
        public LoginViewModel(IUserBusiness userServices)
        {
            UserServices = userServices;
        }

        public async Task<bool> Login(string email, string password)
        {
            var isSuccess = await UserServices.LoginAsync(email, password);
            return isSuccess;
        }
    }
}
