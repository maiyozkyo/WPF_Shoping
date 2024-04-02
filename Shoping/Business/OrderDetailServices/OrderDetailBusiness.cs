using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.OrderDetailServices
{
    public class OrderDetailBusiness : BaseBusiness<OrderDetail>, IOrderDetailBusiness
    {
        public OrderDetailBusiness(string _dbName) : base(_dbName)
        {
        }
        public async Task<List<OrderDetailDTO>> GetOrderDetailsInRange(DateTime from, DateTime to)
        {
            var lstOrderDetails = await Repository.GetAsync(c => c.CreatedOn >= from && c.CreatedOn <= to).ToListAsync();
            return JsonConvert.DeserializeObject<List<OrderDetailDTO>>(JsonConvert.SerializeObject(lstOrderDetails));
        }
    }
}
