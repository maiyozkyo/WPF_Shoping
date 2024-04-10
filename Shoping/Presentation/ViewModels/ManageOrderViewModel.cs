using PropertyChanged;
using Shoping.Business.CustomerServices;
using Shoping.Business.OrderDetailServices;
using Shoping.Business.OrderServices;
using Shoping.Business.VoucherServices;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System.ComponentModel;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ManageOrderViewModel
    {
        public IOrderBusiness OrderBusiness;
        public ICustomerBusiness CustomerBusiness;
        public IOrderDetailBusiness OrderDetailBusiness;
        public IVoucherBusiness VoucherBusiness { get; set; }
        public List<OrderDTO> orders { get; set; }
        public List<OrderDetailDTO> orderDetails { get; set; }
        public BindingList<VoucherDTO> ListVoucherDTOs { get; set; }
        public ManageOrderViewModel(IOrderBusiness orderBusiness, ICustomerBusiness customerBusiness, IOrderDetailBusiness orderDetailBusiness, IVoucherBusiness voucherBusiness)
        {
            OrderBusiness = orderBusiness;
            CustomerBusiness = customerBusiness;
            OrderDetailBusiness = orderDetailBusiness;
            VoucherBusiness = voucherBusiness;
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
        public async Task<bool> AddUpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO, Guid orderId)
        {
            var result = await OrderDetailBusiness.AddUpdateOrderDetailAsync(orderDetailDTO, orderId);
            return result != Guid.Empty;
        }
        public async Task<double> DeleteOrderDetail(OrderDetailDTO orderDetailDTO, Guid orderId)
        {
            var result = await OrderDetailBusiness.DeleteOrderDetailsAsync(orderDetailDTO.ProductID, orderId);
            return result;
        }
        public async Task<List<OrderDetailDTO>> GetAllOrderDetails(Guid orderId)
        {
            orderDetails = await OrderDetailBusiness.GetAllOrderDetails(orderId);
            return orderDetails;
        }

        // Order
        public async Task<Guid> AddUpdateOrderAsync(OrderDTO orderDTO)
        {
            var result = await OrderBusiness.AddUpdateOrderAsync(orderDTO);
            return result;
        }
        public async Task<bool> DeleteOrder(OrderDTO orderDTO)
        {
            List<OrderDetailDTO> _list = await OrderDetailBusiness.GetAllOrderDetails(orderDTO.RecID);
            foreach (var orderDetail in _list)
            {
                await OrderDetailBusiness.DeleteOrderDetailsByOrder(orderDTO.RecID);
            }
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

        //Vouchers
        public async Task GetVouchers()
        {
            var voucherDTOs = await VoucherBusiness.GetVouchers(true);
            ListVoucherDTOs = new BindingList<VoucherDTO>(voucherDTOs);
        }
    }
}
