using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DTOs
{
    public class PagingDTO<TEntity> where TEntity : class
    {
        public int Total { get; set; }
        public List<TEntity> Data { get; set; }
    }
}
