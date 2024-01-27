using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public interface IUserBusiness
    {
        public Task<User> AddUpdateUserAsync(User user);
        public Task<User> GetUserAsync(string email, string password);
    }
}
