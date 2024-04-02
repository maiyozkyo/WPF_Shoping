using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DTOs
{
    public class CategoryDTO : ICloneable
    {
        public Guid RecID { get; set; }
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
