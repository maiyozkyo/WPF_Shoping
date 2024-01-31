﻿using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.OderServices
{
    public interface IOrderServices
    {
        public Task<Guid> AddUpdateOrderAsync(OrderDTO orderDTO);
        public Task<bool> DeleteOrderAsync(Guid orderRecID);
        public Task<OrderDTO> GetOrderAsync(Guid orderRecID);
        public Task<List<OrderDTO>> GetOrdersInRangeAsync(DateTime from, DateTime to);
    }
}
