using Microsoft.Extensions.Configuration;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business
{
    public class UserBusiness : BaseBusiness<User>
    {
        private readonly IConfiguration _iConfiguration;
        public UserBusiness(IConfiguration configuration) : base(configuration)
        {
            _iConfiguration = configuration;
        }
    }
}
