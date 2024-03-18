using PropertyChanged;
using Shoping.Business.OderServices;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        public IOrderBusiness OrderBusiness;
        public MainViewModel(IOrderBusiness orderBusiness)
        {
            OrderBusiness = orderBusiness;
        }

        public async Task<Guid> AddUpdateOrder(OrderDTO orderDTO)
        {
            var result = await OrderBusiness.AddUpdateOrderAsync(orderDTO);
            return result;
        }

        public async Task<PageData<OrderDTO>> Paging(int page, int pageSize)
        {
            return await OrderBusiness.GetOrderPaging(page, pageSize);
        }
    }
}
 