using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Shoping.Presentation.View.order
{
    public partial class DataEditControlxaml : Window
    {
        ObservableCollection<OrderDetailDTO> _list;
        public int SelectedIndex { get; set; }
        public OrderDTO EditOrder { get; set; }
        public ObservableCollection<OrderDetailDTO> _listOrderDetail { get; set; }

        public event EventHandler<DataInputEventArgs> DataInputCompleted;
        public event EventHandler DataUpdated;
        public ManageOrderViewModel ManageOrderViewModel { get; set; }
        public MainViewModel MainViewModel { get; set; }
        double _totalMoney;
        public DataEditControlxaml(int selectedIndex, OrderDTO editOrder)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ManageOrderViewModel = new ManageOrderViewModel(App.iOrderBusiness, App.iCustomerBusiness, App.iOrderDetailBusiness, App.iVoucherBusiness);
            MainViewModel = new MainViewModel(App.iProductBusiness);
            DataContext = ManageOrderViewModel;
            DataContext = MainViewModel;

            SelectedIndex = selectedIndex;
            EditOrder = editOrder;
            GetCustomerData(editOrder.CustomerID);
            total_money.Text = editOrder.TotalMoney.ToString();
            delivery_date.SelectedDate = editOrder.DeliveryDate;
            payment_status.IsChecked = editOrder.PaymentStatus;
        }
        private async void GetCustomerData(Guid customerId)
        {
            CustomerDTO customerDTO = await ManageOrderViewModel.GetCustomerById(customerId);
            customer_name.Text = $"{customerDTO.FirstName} {customerDTO.LastName}";
        }
        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private async void LoadData()
        {
            _list = new ObservableCollection<OrderDetailDTO>();
            List<OrderDetailDTO> _orderDetails = new List<OrderDetailDTO>();
            _orderDetails = await ManageOrderViewModel.GetAllOrderDetails(EditOrder.RecID);
            foreach (var orderDetailDTO in _orderDetails)
            {
                _list.Add(new OrderDetailDTO
                {
                    ProductID = orderDetailDTO.ProductID,
                    Image = orderDetailDTO.Image,
                    NameProduct = orderDetailDTO.NameProduct,
                    Quantity = orderDetailDTO.Quantity,
                    Price = orderDetailDTO.Price,
                    Total = orderDetailDTO.Total,
                });
            }
            PhoneComboBox.ItemsSource = _list;
        }
        private async void EditCartData_DataInputCompleted(object sender, CartInputEventArgs e)
        {
            _totalMoney = e.TotalMoney + double.Parse(total_money.Text);
            total_money.Text = _totalMoney.ToString();
            _listOrderDetail = e.ListOrderDetail;
        }
        private void EditOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            ProductUI productUI = new ProductUI();
            productUI.CartInputCompleted += EditCartData_DataInputCompleted;
            productUI.Closed += ProductUI_Closed;
            productUI.ShowDialog();
        }
        private async void ProductUI_Closed(object sender, EventArgs e)
        {
            foreach (var orderDetailDTO in _listOrderDetail)
            {
                await ManageOrderViewModel.AddUpdateOrderDetailAsync(orderDetailDTO, EditOrder.RecID);
            }
            MessageBox.Show($"Cập nhật giỏ hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }
        private async void DeleteOrderDetail_Click(Object sender, RoutedEventArgs e)
        {
            int i = PhoneComboBox.SelectedIndex;
            List<OrderDetailDTO> _orderDetails = new List<OrderDetailDTO>();
            _orderDetails = await ManageOrderViewModel.GetAllOrderDetails(EditOrder.RecID);
            OrderDetailDTO _orderDetailDTO = _orderDetails[i];

            double totalDeleted = await ManageOrderViewModel.DeleteOrderDetail(_orderDetailDTO, EditOrder.RecID);
            total_money.Text = ((double.Parse(total_money.Text) - totalDeleted).ToString());
            MessageBox.Show($"Xoá khỏi giỏ hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }
        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            double totalMoney = double.Parse(total_money.Text);
            DateTime? dateDelivery = delivery_date.SelectedDate;
            bool paymentStatus = payment_status.IsChecked ?? false;

            if (totalMoney <= 0)
            {
                await ManageOrderViewModel.DeleteOrder(EditOrder);
                DataInputCompleted?.Invoke(this, null);
            }
            else
            {
                DataInputCompleted?.Invoke(this, new DataInputEventArgs(totalMoney, dateDelivery.Value, paymentStatus, SelectedIndex));
            }
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
    public class DataInputEventArgs
    {
        public double TotalMoney { get; }
        public DateTime DeliveryDate { get; }
        public bool PaymentStatus { get; }
        public int SelectedIndex { get; }
        public DataInputEventArgs(double total_money, DateTime delivery_date, bool payment_status, int selectedIndex)
        {
            TotalMoney = total_money;
            DeliveryDate = delivery_date;
            PaymentStatus = payment_status;
            SelectedIndex = selectedIndex;
        }
    }
}
