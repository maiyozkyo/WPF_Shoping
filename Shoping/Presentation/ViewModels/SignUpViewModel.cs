using PropertyChanged;
using Shoping.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SignUpViewModel
    {
        public IUserBusiness UserBusiness;
        public SignUpViewModel(IUserBusiness userBusiness)
        {
            UserBusiness = userBusiness;
        }
    }
}
