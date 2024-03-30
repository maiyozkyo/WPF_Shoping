using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.OrderDetailServices
{
    public interface IOrderDetailBusiness
    {
        public Task<List<OrderDetailDTO>> GetOrderDetailsInRange(DateTime from, DateTime to);
    }
}
