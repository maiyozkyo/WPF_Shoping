using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Data_Access.DB.Repo
{
    public static class CustomIQueryable
    {
        public static async Task<PageData<DestinationType>> ToPaging<ResourceType, DestinationType>(this IQueryable<ResourceType> query, int pageNumber, int pageSize) where DestinationType : class
        {
            var lstResources = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var page = new PageData<DestinationType>
            {
                Total = await query.CountAsync(),
                Data = JsonConvert.DeserializeObject<List<DestinationType>>(JsonConvert.SerializeObject(lstResources)),
            };
            return page;
        }
    }
}
