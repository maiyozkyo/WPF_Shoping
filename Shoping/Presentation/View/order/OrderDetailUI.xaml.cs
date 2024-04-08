using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;


namespace Shoping.Presentation.View.order
{
    public partial class OrderDetailUI : Window
    {
        ObservableCollection<OrderDetailDTO> _list;
        public int SelectedIndex { get; set; }
        public OrderDTO EditOrder { get; set; }
        public ObservableCollection<OrderDetailDTO> _listOrderDetail { get; set; }

        public event EventHandler<DataInputEventArgs> DataInputCompleted;
        public ManageOrderViewModel ManageOrderViewModel { get; set; }
        public MainViewModel MainViewModel { get; set; }
        private void OrderDetail_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public OrderDetailUI(int selectedIndex, OrderDTO editOrder)
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
            delivery_date.Text = editOrder.DeliveryDate.ToString("g");
            if (editOrder.PaymentStatus)
            {
                payment_status.Text = "Đã giao hàng";
            }
            else
            {
                payment_status.Text = "Chưa giao hàng";
            }
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
        private void back_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}
