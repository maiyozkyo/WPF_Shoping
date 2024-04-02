using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;


namespace Shoping.Presentation.View.order
{
    public partial class ProductUI : Window
    {
        ObservableCollection<Phone> _list;
        public event EventHandler<double> ProductAddedToCart;
        public event EventHandler<CartInputEventArgs> CartInputCompleted;
        public double _totalMoney = 0;
        private bool _addItemClicked = false;
        public MainViewModel MainViewModel { get; set; }


        public ProductUI()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainViewModel = new MainViewModel(App.iProductBusiness);
        }

        private async void Product_Loaded(object sender, RoutedEventArgs e)
        {

            //_list = new ObservableCollection<ProductDTO>();
            _list = new ObservableCollection<Phone>();

            _list.Add(new Phone() { Name = "iPhone 15 Pro Max", Price = 31990000, Manufacturer = "Samsung", Avatar = "Images/mobile01.jpg" });
            _list.Add(new Phone() { Name = "Samsung Galaxy S24+ 5G", Price = 26990000, Manufacturer = "Samsung", Avatar = "Images/mobile02.jpg" });
            _list.Add(new Phone() { Name = "Oppo Reno 11 5G", Price = 10690000, Manufacturer = "Oppo", Avatar = "Images/mobile03.jpg" });
            _list.Add(new Phone() { Name = "Xiaomi Redmi Note 13", Price = 4990000, Manufacturer = "Xiaomi", Avatar = "Images/mobile04.jpg" });
            _list.Add(new Phone() { Name = "vivo Y17s", Price = 4190000, Manufacturer = "Vivo", Avatar = "Images/mobile05.jpg" });
            _list.Add(new Phone() { Name = "realme Note 50 128GB", Price = 2890000, Manufacturer = "Oppo", Avatar = "Images/mobile06.jpg" });
            _list.Add(new Phone() { Name = "iphone 13", Price = 14990000, Manufacturer = "Apple", Avatar = "Images/mobile07.jpg" });
            _list.Add(new Phone() { Name = "realme C55", Price = 4190000, Manufacturer = "Oppo", Avatar = "Images/mobile08.jpg" });
            _list.Add(new Phone() { Name = "iphone 11", Price = 9990000, Manufacturer = "Apple", Avatar = "Images/mobile09.jpg" });
            _list.Add(new Phone() { Name = "Xiaomi Redmi Note 13 Pro 5G", Price = 9190000, Manufacturer = "Xiaomi", Avatar = "Images/mobile10.jpg" });

            //List<ProductDTO> _products = new List<ProductDTO>();
            //_products = await MainViewModel.GetAllProducts();
            //foreach (var productDTO in _products)
            //{
            //    _list.Add(new ProductDTO
            //    {
            //        RecID = productDTO.RecID,
            //        ProductID = productDTO.ProductID,
            //        Name = productDTO.Name,
            //        Price = productDTO.Price,
            //        PurchasePrice = productDTO.PurchasePrice,
            //        CatID = productDTO.CatID,
            //        Quantity = productDTO.Quantity,
            //        Image = productDTO.Image
            //    });
            //}

            PhoneComboBox.ItemsSource = _list;
        }
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            int i = PhoneComboBox.SelectedIndex;
            //_totalMoney += _list[i].Price;

            _addItemClicked = true;
        }
        private void Completed_Click(object sender, RoutedEventArgs e)
        {
            if (_addItemClicked)
            {
                CartInputCompleted?.Invoke(this, new CartInputEventArgs(_totalMoney));
                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
        }
    }
    public class CartInputEventArgs
    {
        public double TotalMoney { get; }
        public CartInputEventArgs(double total_money)
        {
            TotalMoney = total_money;
        }
    }
}
