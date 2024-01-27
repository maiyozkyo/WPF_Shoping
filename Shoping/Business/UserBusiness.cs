using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public class UserBusiness : BaseBusiness<User>, IUserBusiness
    {
        public UserBusiness(string _dbName) : base(_dbName)
        {
        }

        public async Task<User> AddUpdateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    return null;
                }

                var addedUser = await Repository.GetOneAsync(x => x.Email == user.Email);
                if (addedUser != null)
                {
                    addedUser.ModifiedOn = DateTime.Now;
                    return addedUser;
                }
                else
                {
                    user.CreatedOn = DateTime.Now;
                    Repository.Add(user);
                }
                await UnitOfWork.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<User> GetUserAsync(string email, string password)
        {
            var user = await Repository.GetOneAsync(x => x.Email == email && password == x.Password);
            return user;
        }

    }
}
