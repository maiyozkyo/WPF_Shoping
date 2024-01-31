using PropertyChanged;
using Shoping.Business.UserServices;
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
        public IUserServices UserServices;
        public SignUpViewModel(IUserServices userServices)
        {
            UserServices = userServices;
        }
    }
}
