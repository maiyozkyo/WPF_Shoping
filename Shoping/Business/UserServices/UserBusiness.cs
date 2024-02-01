using Newtonsoft.Json;
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
            if (user == null)
            {
                return null;
            }
            var dictObj = new Dictionary<string, object>
            {
                {
                    "User", JsonConvert.SerializeObject(user)
                }
            };
            var res = await ApiService.Post<UserDTO>($"{ApiService.UserUrl}/AddUpdate", dictObj);
            return res;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await GetUserLoginAsync(email, password);
            if (user != null)
            {
                App.SetAuth(user);
                return true;
            }
            return false;
        }

        private async Task<UserDTO> GetUserLoginAsync(string email, string password)
        {
            var dictObj = new Dictionary<string, object>
            {
                { "Email", email },
                { "Password", password }
            };
            var res = await ApiService.Post<UserDTO>($"{ApiService.UserUrl}/Login", dictObj);
            return res;
        }

       
    }
}
