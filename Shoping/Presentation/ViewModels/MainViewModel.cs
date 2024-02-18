using PropertyChanged;
using Shoping.Business.OderServices;
using Shoping.Data_Access.DTOs;
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

        public async Task<Guid> AddUpdateOrder(OrderDTO orderDTO = null)
        {
            //nho xoa
            if (orderDTO == null)
            {
                orderDTO = new OrderDTO
                {
                    RecID = Guid.Parse("7f425855-f373-4c65-8ac7-82e2cb0cc871"),
                    CustomerID = Guid.NewGuid(),
                    Paid = 100,
                    Total = 1000,
                };
            }
            var result = await OrderBusiness.AddUpdateOrderAsync(orderDTO);
            return result;
        }
    }
}
 