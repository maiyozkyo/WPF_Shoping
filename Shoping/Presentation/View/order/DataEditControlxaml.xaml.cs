using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Shoping.Presentation.View.order
{
    public partial class DataEditControlxaml : Window
    {
        ObservableCollection<Phone> _list;
        public int SelectedIndex { get; set; }
        public OrderDTO EditOrder { get; set; }

        public event EventHandler<DataInputEventArgs> DataInputCompleted;
        public ManageOrderViewModel ManageOrderViewModel { get; set; }
        private void Product_Loaded(object sender, RoutedEventArgs e)
        {
            _list = new ObservableCollection<Phone>();

            _list.Add(new Phone() { Name = "iPhone 15 Pro Max", Price = 31990000, Manufacturer = "Samsung", Avatar = "images/mobile01.jpg" });
            _list.Add(new Phone() { Name = "Samsung Galaxy S24+ 5G", Price = 26990000, Manufacturer = "Samsung", Avatar = "images/mobile02.jpg" });
            _list.Add(new Phone() { Name = "Oppo Reno 11 5G", Price = 10690000, Manufacturer = "Oppo", Avatar = "images/mobile03.jpg" });
            _list.Add(new Phone() { Name = "Xiaomi Redmi Note 13", Price = 4990000, Manufacturer = "Xiaomi", Avatar = "images/mobile04.jpg" });
            _list.Add(new Phone() { Name = "vivo Y17s", Price = 4190000, Manufacturer = "Vivo", Avatar = "images/mobile05.jpg" });
            _list.Add(new Phone() { Name = "realme Note 50 128GB", Price = 2890000, Manufacturer = "Oppo", Avatar = "images/mobile06.jpg" });
            _list.Add(new Phone() { Name = "iphone 13", Price = 14990000, Manufacturer = "Apple", Avatar = "images/mobile07.jpg" });
            _list.Add(new Phone() { Name = "realme C55", Price = 4190000, Manufacturer = "Oppo", Avatar = "images/mobile08.jpg" });
            _list.Add(new Phone() { Name = "iphone 11", Price = 9990000, Manufacturer = "Apple", Avatar = "images/mobile09.jpg" });
            _list.Add(new Phone() { Name = "Xiaomi Redmi Note 13 Pro 5G", Price = 9190000, Manufacturer = "Xiaomi", Avatar = "images/mobile10.jpg" });

            PhoneComboBox.ItemsSource = _list;
        }
        public DataEditControlxaml(int selectedIndex, OrderDTO editOrder)
        {
            InitializeComponent();
            ManageOrderViewModel = new ManageOrderViewModel(App.iOrderBusiness, App.iCustomerBusiness);
            DataContext = ManageOrderViewModel;

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
        private void DisplayProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductUI productUI = new ProductUI();
            productUI.Closed += HandleProductUIClosed;
            productUI.Show();
        }
        private void HandleProductUIClosed(object sender, EventArgs e)
        {

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
