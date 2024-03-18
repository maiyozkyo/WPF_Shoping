using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.Models
{
    public class PageData<T> where T : class
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
