﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class User : MongoDBEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
