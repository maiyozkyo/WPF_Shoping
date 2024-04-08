using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using Shoping.Presentation.View.order;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Shoping.Presentation.Control
{
    public partial class OrderUserControl : UserControl
    {
        public ManageOrderViewModel ManageOrderViewModel { get; set; }
        public ObservableCollection<OrderDTO> _orders { get; set; }
        public ObservableCollection<OrderDetailDTO> _listOrderDetail { get; set; }
        public double OriginalTotal = 0;
        public int pageIndex { get; set; }
        PagingInfo _paging;
        bool flag = false;
        class PagingInfo
        {
            public int currentPage { get; set; }
            public int totalPage { get; set; }
            public string DisplayText => $"{currentPage}/{totalPage}";
        }
        public OrderUserControl()
        {
            InitializeComponent();
            ManageOrderViewModel = new ManageOrderViewModel(App.iOrderBusiness, App.iCustomerBusiness, App.iOrderDetailBusiness, App.iVoucherBusiness);
            DataContext = ManageOrderViewModel;
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private async void LoadData()
        {
            PageData<OrderDTO> paging = await ManageOrderViewModel.Paging(1, 4);

            _orders = new ObservableCollection<OrderDTO>();
            foreach (var orderDTO in paging.Data)
            {
                var order = new OrderDTO
                {
                    RecID = orderDTO.RecID,
                    CustomerID = orderDTO.CustomerID,
                    TotalMoney = orderDTO.TotalMoney,
                    DeliveryDate = orderDTO.DeliveryDate,
                    PaymentStatus = orderDTO.PaymentStatus,
                };
                _orders.Add(order);
            }
            DataContext = ManageOrderViewModel;

            OrderComboBox.ItemsSource = _orders;

            _paging = new PagingInfo();

            _paging.currentPage = 1;
            _paging.totalPage = (paging.Total > 0 && paging.Data.Count() > 0) ?
                    (paging.Total % paging.Data.Count() == 0) ?
                    paging.Total / paging.Data.Count() :
                    paging.Total / paging.Data.Count() + 1 :
                    1;

            var infos = new ObservableCollection<PagingInfo>();
            for (int i = 1; i <= _paging.totalPage; i++)
            {
                infos.Add(new PagingInfo
                {
                    currentPage = i,
                    totalPage = _paging.totalPage
                });
            }

            pageIndex = 1;
            pageTextBox.Text = pageIndex.ToString() + " / " + $"{_paging.totalPage}";

            await ManageOrderViewModel.GetVouchers();
            CbxVouchers.ItemsSource = ManageOrderViewModel.ListVoucherDTOs;
        }
        private async void loadDataPerPage(int page)
        {
            PageData<OrderDTO> paging = await ManageOrderViewModel.Paging(page, 4);

            _orders = new ObservableCollection<OrderDTO>();
            foreach (var orderDTO in paging.Data)
            {
                var order = new OrderDTO
                {
                    RecID = orderDTO.RecID,
                    CustomerID = orderDTO.CustomerID,
                    TotalMoney = orderDTO.TotalMoney,
                    DeliveryDate = orderDTO.DeliveryDate,
                    PaymentStatus = orderDTO.PaymentStatus,
                };
                _orders.Add(order);
            }

            OrderComboBox.ItemsSource = _orders;
        }
        private async void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var customerDTO = new CustomerDTO();

            // initialize new customer
            customerDTO.FirstName = first_name.Text;
            customerDTO.LastName = last_name.Text;
            Guid customerId = Guid.Empty;
            try
            {
                customerId = await ManageOrderViewModel.CreateCustomer(customerDTO);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // assign value to order
            double totalMoney = double.Parse(total_money.Text);
            DateTime? dateDelivery = delivery_date.SelectedDate;
            bool paymentStatus = payment_status.IsChecked ?? false;
            DateTime createOn = DateTime.Now;

            if (totalMoney > 0 && delivery_date.SelectedDate != null)
            {
                var orderDTO = new OrderDTO()
                {
                    CustomerID = customerId,
                    TotalMoney = totalMoney,
                    DeliveryDate = dateDelivery.Value,
                    PaymentStatus = paymentStatus,
                };

                Guid orderId = await ManageOrderViewModel.AddUpdateOrderAsync(orderDTO);

                if (orderId != Guid.Empty)
                {
                    foreach (var orderDetailDTO in _listOrderDetail)
                    {
                        await ManageOrderViewModel.AddUpdateOrderDetailAsync(orderDetailDTO, orderId);
                    }
                    MessageBox.Show($"Tạo đơn hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    flag = false;
                    LoadData();
                }
                ResetInputData();
            }
            else
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ReloadedOrder_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            LoadData();
        }
        private void DetailOrder_Click(object sender, RoutedEventArgs e)
        {
            int i = OrderComboBox.SelectedIndex;
            OrderDTO detailOrderDTO = _orders[i];

            OrderDetailUI orderDetailUI = new OrderDetailUI(i, detailOrderDTO);
            orderDetailUI.ShowDialog();
        }
        private async void EditData_DataInputCompleted(object sender, DataInputEventArgs e)
        {
            if (e != null)
            {
                _orders[e.SelectedIndex].TotalMoney = e.TotalMoney;
                _orders[e.SelectedIndex].DeliveryDate = e.DeliveryDate;
                _orders[e.SelectedIndex].PaymentStatus = e.PaymentStatus;

                await ManageOrderViewModel.AddUpdateOrderAsync(_orders[e.SelectedIndex]);
            }
            flag = false;
            LoadData();
        }
        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = OrderComboBox.SelectedIndex;
            OrderDTO selectedOrderDTO = _orders[selectedIndex];

            DataEditControlxaml newDataInputControl = new DataEditControlxaml(selectedIndex, selectedOrderDTO);
            newDataInputControl.DataInputCompleted += EditData_DataInputCompleted;
            newDataInputControl.ShowDialog();
        }
        private void ResetCart_Click(object sender, RoutedEventArgs e)
        {
            total_money.Text = "0";
            _listOrderDetail = null;
        }
        private async void EditCartData_DataInputCompleted(object sender, CartInputEventArgs e)
        {
            total_money.Text = e.TotalMoney.ToString();
            double.TryParse(total_money.Text, out OriginalTotal);
            CbxVouchers_SelectionChanged(null, null);
            _listOrderDetail = e.ListOrderDetail;
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            ProductUI productUI = new ProductUI();
            productUI.CartInputCompleted += EditCartData_DataInputCompleted;
            productUI.ShowDialog();
        }
        private async void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            int i = OrderComboBox.SelectedIndex;
            OrderDTO deletedOrderDTO = _orders[i];

            await ManageOrderViewModel.DeleteOrder(deletedOrderDTO);
            MessageBox.Show($"Huỷ đơn thành công", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
            flag = false;
            LoadData();
        }

        private async void LoadSearchData(DateTime fromDate, DateTime toDate)
        {
            PageData<OrderDTO> paging = await ManageOrderViewModel.SearchOrder(fromDate, toDate, 1, 4);

            if (paging != null)
            {
                if (paging.Data.Count() == 0)
                {
                    return;
                }
                _orders.Clear();

                _orders = new ObservableCollection<OrderDTO>();
                foreach (var orderDTO in paging.Data)
                {
                    var order = new OrderDTO
                    {
                        RecID = orderDTO.RecID,
                        CustomerID = orderDTO.CustomerID,
                        TotalMoney = orderDTO.TotalMoney,
                        DeliveryDate = orderDTO.DeliveryDate,
                        PaymentStatus = orderDTO.PaymentStatus,
                    };
                    _orders.Add(order);
                }
                OrderComboBox.ItemsSource = _orders;

                DataContext = ManageOrderViewModel;

                _paging = new PagingInfo();

                _paging.currentPage = 1;
                _paging.totalPage = (paging.Total > 0 && paging.Data.Count() > 0) ?
                    (paging.Total % paging.Data.Count() == 0) ?
                    paging.Total / paging.Data.Count() :
                    paging.Total / paging.Data.Count() + 1 :
                    1;

                var infos = new ObservableCollection<PagingInfo>();
                for (int i = 1; i <= _paging.totalPage; i++)
                {
                    infos.Add(new PagingInfo
                    {
                        currentPage = i,
                        totalPage = _paging.totalPage
                    });
                }

                pageIndex = 1;
                pageTextBox.Text = pageIndex.ToString() + " / " + $"{_paging.totalPage}";

                flag = true;
            }
            else
            {
                flag = false;
            }
        }
        private async void loadDataSearchPerPage(int page, DateTime fromDate, DateTime toDate)
        {
            PageData<OrderDTO> paging = await ManageOrderViewModel.SearchOrder(fromDate, toDate, page, 4);

            _orders = new ObservableCollection<OrderDTO>();
            foreach (var orderDTO in paging.Data)
            {
                var order = new OrderDTO
                {
                    RecID = orderDTO.RecID,
                    CustomerID = orderDTO.CustomerID,
                    TotalMoney = orderDTO.TotalMoney,
                    DeliveryDate = orderDTO.DeliveryDate,
                    PaymentStatus = orderDTO.PaymentStatus,
                };
                _orders.Add(order);
            }

            OrderComboBox.ItemsSource = _orders;
        }
        DateTime? dateBefore;
        DateTime? dateAfter;
        private async void SearchOrder_Click(object sender, RoutedEventArgs e)
        {
            dateBefore = date_before.SelectedDate;
            dateAfter = date_after.SelectedDate;

            if (dateBefore.HasValue && dateAfter.HasValue)
            {
                LoadSearchData(dateBefore.Value, dateAfter.Value);
                if (flag == true)
                {
                    MessageBox.Show($"Tìm thành công", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy đơn hàng hợp lệ", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập ngày để tìm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            date_before.SelectedDate = null;
            date_after.SelectedDate = null;
        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_paging.currentPage < _paging.totalPage)
            {
                _paging.currentPage++;
                pageIndex++;
                pageTextBox.Text = pageIndex.ToString() + " / " + $"{_paging.totalPage}";
                if (flag == true)
                {
                    loadDataSearchPerPage(_paging.currentPage, dateBefore.Value, dateAfter.Value);
                }
                else
                {
                    loadDataPerPage(_paging.currentPage);
                }
            }
        }
        private void previousButton_Click(Object sender, RoutedEventArgs e)
        {
            if (_paging.currentPage > 1)
            {
                _paging.currentPage--;
                pageIndex--;
                pageTextBox.Text = pageIndex.ToString() + " / " + $"{_paging.totalPage}";
                if (flag == true)
                {
                    loadDataSearchPerPage(_paging.currentPage, dateBefore.Value, dateAfter.Value);
                }
                else
                {
                    loadDataPerPage(_paging.currentPage);
                }
            }
        }
        private void ResetInputData()
        {
            first_name.Text = "";
            last_name.Text = "";
            total_money.Text = "";
            delivery_date.SelectedDate = null;
            payment_status.IsChecked = false;
        }

        private void CbxVouchers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item
            var voucher = CbxVouchers.SelectedItem as VoucherDTO;
            if (OriginalTotal > 0 && voucher != null)
            {
                var discount = voucher.IsPercent ? (OriginalTotal * (voucher.Value / 100)) : voucher.Value;
                var total = OriginalTotal - (voucher.Max > 0 ? Math.Min(discount, voucher.Max) : discount);
                total_money.Text = total.ToString();
            }
        }
    }
}
