using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Shoping.Presentation.View.order
{
    public partial class DataEditControlxaml : Window
    {
        ObservableCollection<ProductDTO> _list;
        public int SelectedIndex { get; set; }
        public OrderDTO EditOrder { get; set; }

        public event EventHandler<DataInputEventArgs> DataInputCompleted;
        public ManageOrderViewModel ManageOrderViewModel { get; set; }
        public MainViewModel MainViewModel { get; set; }
        public DataEditControlxaml(int selectedIndex, OrderDTO editOrder)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ManageOrderViewModel = new ManageOrderViewModel(App.iOrderBusiness, App.iCustomerBusiness, App.iOrderDetailBusiness);
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
        private async void Product_Loaded(object sender, RoutedEventArgs e)
        {
            _list = new ObservableCollection<ProductDTO>();
            List<ProductDTO> _products = new List<ProductDTO>();
            _products = await MainViewModel.GetAllProducts();
            foreach (var productDTO in _products)
            {
                _list.Add(new ProductDTO
                {
                    RecID = productDTO.RecID,
                    ProductID = productDTO.ProductID,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    PurchasePrice = productDTO.PurchasePrice,
                    CatID = productDTO.CatID,
                    Quantity = productDTO.Quantity,
                    Image = productDTO.Image
                });
            }
            PhoneComboBox.ItemsSource = _list;
        }
        private async void EditCartData_DataInputCompleted(object sender, CartInputEventArgs e)
        {
            total_money.Text = e.TotalMoney.ToString();
        }
        private void EditOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            ProductUI productUI = new ProductUI();
            productUI.CartInputCompleted += EditCartData_DataInputCompleted;
            productUI.ShowDialog();
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            double totalMoney = double.Parse(total_money.Text);
            DateTime? dateDelivery = delivery_date.SelectedDate;
            bool paymentStatus = payment_status.IsChecked ?? false;

            DataInputCompleted?.Invoke(this, new DataInputEventArgs(totalMoney, dateDelivery.Value, paymentStatus, SelectedIndex));
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
