using Shoping.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    public class LoginViewModel
    {
        public IUserBusiness UserBusiness;
        public LoginViewModel(IUserBusiness userBusiness)
        {
            UserBusiness = userBusiness;
        }
    }
}
