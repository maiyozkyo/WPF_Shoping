using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;


namespace Shoping.Presentation.View.order
{
    public partial class ProductUI : Window
    {
        ObservableCollection<ProductDTO> _list;
        public event EventHandler<CartInputEventArgs> CartInputCompleted;
        public double _totalMoney = 0;
        ObservableCollection<OrderDetailDTO> _listOrderDetail;
        public MainViewModel MainViewModel { get; set; }
        public ProductUI()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainViewModel = new MainViewModel(App.iProductBusiness);
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
            _listOrderDetail = new ObservableCollection<OrderDetailDTO>();
            PhoneComboBox.ItemsSource = _list;
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            int i = PhoneComboBox.SelectedIndex;

            double quantity;
            if (double.TryParse(quantity_product.Text, out quantity))
            {
                if (quantity > 0)
                {
                    MessageBox.Show($"Thêm vào đơn hàng thành công", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                    _totalMoney += (double)_list[i].Price * quantity;

                    // create order detail
                    OrderDetailDTO orderDetail = new OrderDetailDTO
                    {
                        ProductID = _list[i].ProductID,
                        Quantity = quantity,
                        Price = _list[i].Price,
                        Total = _list[i].Price * quantity,
                    };
                    _listOrderDetail.Add(orderDetail);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            quantity_product.Text = "";
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CartInputCompleted?.Invoke(this, new CartInputEventArgs(_totalMoney, _listOrderDetail));
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
    public class CartInputEventArgs
    {
        public double TotalMoney { get; }
        public ObservableCollection<OrderDetailDTO> ListOrderDetail { get; }
        public CartInputEventArgs(double total_money, ObservableCollection<OrderDetailDTO> list_orderDetail)
        {
            TotalMoney = total_money;
            ListOrderDetail = list_orderDetail;
        }
    }
}
