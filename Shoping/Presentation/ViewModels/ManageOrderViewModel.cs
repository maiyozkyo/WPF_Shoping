using PropertyChanged;
using Shoping.Business.CustomerServices;
using Shoping.Business.OrderServices;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;


namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ManageOrderViewModel
    {
        public IOrderBusiness OrderBusiness;
        public ICustomerBusiness CustomerBusiness;
        public List<OrderDTO> orders { get; set; }
        public ManageOrderViewModel(IOrderBusiness orderBusiness, ICustomerBusiness customerBusiness)
        {
            OrderBusiness = orderBusiness;
            CustomerBusiness = customerBusiness;
        }
        public async Task<Guid> CreateCustomer(CustomerDTO customerDTO)
        {
            var result = await CustomerBusiness.CreateCustomer(customerDTO);
            return result;
        }
        public async Task<CustomerDTO> GetCustomerById(Guid customerId)
        {
            var result = await CustomerBusiness.GetCustomerById(customerId);
            return result;
        }
        public async Task<bool> AddUpdateOrderAsync(OrderDTO orderDTO)
        {
            var result = await OrderBusiness.AddUpdateOrderAsync(orderDTO);
            return result != Guid.Empty;
        }
        public async Task<bool> DeleteOrder(OrderDTO orderDTO)
        {
            var result = await OrderBusiness.DeleteOrderAsync(orderDTO.RecID);
            return result;
        }
        public async Task<List<OrderDTO>> SearchOrder(DateTime startDate, DateTime endDate)
        {
            var result = await OrderBusiness.GetOrdersInRangeAsync(startDate, endDate);
            return result;
        }
        public async Task<List<OrderDTO>> GetAllOrders()
        {
            orders = await OrderBusiness.GetAllOrders();
            return orders;
        }
        public async Task<PageData<OrderDTO>> Paging(int page, int pageSize)
        {
            return await OrderBusiness.GetOrdersPaging(page, pageSize);
        }
        private double _cartTotalMoney;
        public double CartTotalMoney
        {
            get { return _cartTotalMoney; }
            set
            {
                if (_cartTotalMoney != value)
                {
                    _cartTotalMoney = value;
                }
            }
        }
    }
}
