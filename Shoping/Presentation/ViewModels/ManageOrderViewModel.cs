using PropertyChanged;
using Shoping.Business.CustomerServices;
using Shoping.Business.OrderDetailServices;
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
        public IOrderDetailBusiness OrderDetailBusiness;
        public List<OrderDTO> orders { get; set; }
        public List<OrderDetailDTO> orderDetails { get; set; }
        public ManageOrderViewModel(IOrderBusiness orderBusiness, ICustomerBusiness customerBusiness, IOrderDetailBusiness orderDetailBusiness)
        {
            OrderBusiness = orderBusiness;
            CustomerBusiness = customerBusiness;
            OrderDetailBusiness = orderDetailBusiness;
        }
        // Customer
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
        // Order details
        public async Task<bool> AddUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO, Guid productId)
        {
            var result = await OrderDetailBusiness.AddUpdateOrderDetailAsync(orderDetailDTO, productId);
            return result != Guid.Empty;
        }
        public async Task<bool> DeleteOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            var result = await OrderDetailBusiness.DeleteOrderDetailsAsync(orderDetailDTO.RecID);
            return result;
        }
        public async Task<List<OrderDetailDTO>> GetAllOrderDetails()
        {
            orderDetails = await OrderDetailBusiness.GetAllOrderDetails();
            return orderDetails;
        }
        // Order
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
        public async Task<PageData<OrderDTO>> Paging(int page, int pageSize)
        {
            return await OrderBusiness.GetOrdersPaging(page, pageSize);
        }
        public async Task<PageData<OrderDTO>> SearchOrder(DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            return await OrderBusiness.GetOrdersInRangeAsync(fromDate, toDate, page, pageSize);
        }
    }
}
