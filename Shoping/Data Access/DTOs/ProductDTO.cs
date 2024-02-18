﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DTOs
{
    public class ProductDTO
    {
        public Guid RecID { get; set; }
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; } // Implement later
    }
}
