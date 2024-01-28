using Newtonsoft.Json;
using PropertyChanged;
using Shoping.Business;
using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]                                                                                                                  
    public class LoginViewModel
    {
        public IUserBusiness UserBusiness;
        public UserDTO Author { get; private set; }
        public string UserName { get; set; }

        public LoginViewModel(IUserBusiness userBusiness)
        {
            UserBusiness = userBusiness;
        }

        public async Task<bool> Login(string email, string password)
        {
            var user = await UserBusiness.GetUserAsync(email, password);
            if (user == null)
            {
                return false;
            }

            Author = JsonConvert.DeserializeObject<UserDTO>(JsonConvert.SerializeObject(user));
            return true;
        }
    }
}
