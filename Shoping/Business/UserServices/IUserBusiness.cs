using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.UserServices
{
    public interface IUserBusiness
    {
        public Task<UserDTO> AddUpdateUserAsync(UserDTO user);
        public Task<bool> LoginAsync(string email, string password);
    }
}
