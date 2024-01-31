using Shoping.ApiBusiness;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.UserServices
{
    public class UserBusiness : IUserServices
    {
        public ApiService ApiService { get; set; }
        public UserBusiness()
        {
            ApiService = new ApiService();
        }

        public async Task<UserDTO> AddUpdateUserAsync(UserDTO user)
        {

            return null;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await GetUserAsync(email, password);
            if (user != null)
            {
                App.SetAuth(user);
                return true;
            }
            return false;
        }

        private async Task<UserDTO> GetUserAsync(string email, string password)
        {
            var dictObj = new Dictionary<string, object>
            {
                { "Email", email },
                { "Password", password }
            };
            var res = await ApiService.Post<UserDTO>(ApiService.UserUrl, dictObj);
            return res;
        }

       
    }
}
