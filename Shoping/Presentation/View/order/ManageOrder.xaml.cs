﻿using Shoping.Data_Access.DTOs;
using Shoping.Presentation.View.order;
using Shoping.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Shoping.Presentation.View
{
    public partial class ManageOrder : Window
    {
        public ManageOrderViewModel ManageOrderViewModel { get; set; }
        public ObservableCollection<OrderDTO> _orders { get; set; }
        public ManageOrder()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ManageOrderViewModel = new ManageOrderViewModel(App.iOrderBusiness, App.iCustomerBusiness);
            DataContext = ManageOrderViewModel;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrderFromDb();
        }
        private async Task LoadOrderFromDb()
        {
            List<OrderDTO> orderList = await ManageOrderViewModel.GetAllOrders();

            int idxOrder = 0;
            foreach (var order in orderList)
            {
                order.RecID = orderList[idxOrder++].RecID;
                order.DeliveryDate = order.DeliveryDate.AddHours(7); // Thêm 7 giờ để chuyển đổi từ UTC sang UTC+7
            }

            _orders = new ObservableCollection<OrderDTO>(orderList);
            OrderComboBox.ItemsSource = _orders;
        }
        private async void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var customerDTO = new CustomerDTO();

            // initialize new customer

            //customerDTO.FirstName = first_name.Text;
            //customerDTO.LastName = last_name.Text;
            //Guid customerId = Guid.Empty;
            //try
            //{
            //    customerId = await ManageOrderViewModel.CreateCustomer(customerDTO);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            // assign value to order
            total_money.Text = "0";
            double totalMoney = double.Parse(total_money.Text);
            DateTime? dateDelivery = delivery_date.SelectedDate;
            bool paymentStatus = payment_status.IsChecked ?? false;
            DateTime createAt = DateTime.Now;

            var orderDTO = new OrderDTO()
            {
                CustomerID = Guid.Empty,
                TotalMoney = totalMoney,
                DeliveryDate = dateDelivery.Value,
                PaymentStatus = paymentStatus,
                CreateAt = createAt,
            };

            if (await ManageOrderViewModel.AddUpdateOrderAsync(orderDTO))
            {
                MessageBox.Show($"Tạo đơn hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadOrderFromDb();
            }
            ResetInputData();
        }
        private void DetailOrder_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void EditData_DataInputCompleted(object sender, DataInputEventArgs e)
        {
            _orders[e.SelectedIndex].TotalMoney = e.TotalMoney;
            _orders[e.SelectedIndex].DeliveryDate = e.DeliveryDate;
            _orders[e.SelectedIndex].PaymentStatus = e.PaymentStatus;

            await ManageOrderViewModel.AddUpdateOrderAsync(_orders[e.SelectedIndex]);
            await LoadOrderFromDb();
        }
        private void CartData_DataInputCompleted(object sender, CartInputEventArgs e)
        {
            ManageOrderViewModel.CartTotalMoney = e.TotalMoney;
        }
        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = OrderComboBox.SelectedIndex;
            OrderDTO selectedOrderDTO = _orders[selectedIndex];

            DataEditControlxaml newDataInputControl = new DataEditControlxaml(selectedIndex, selectedOrderDTO);
            newDataInputControl.DataInputCompleted += EditData_DataInputCompleted;

            newDataInputControl.ShowDialog();
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            ProductUI productUI = new ProductUI();
            productUI.CartInputCompleted += CartData_DataInputCompleted;
            productUI.ShowDialog();
        }
        private void ResetCart_Click(object sender, RoutedEventArgs e)
        {
            ManageOrderViewModel.CartTotalMoney = 0;
        }
        private async void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            int i = OrderComboBox.SelectedIndex;
            OrderDTO deletedOrderDTO = _orders[i];

            if (i != -1)
            {
                _orders.RemoveAt(i);
            }

            await ManageOrderViewModel.DeleteOrder(deletedOrderDTO);
            MessageBox.Show($"Huỷ đơn thành công", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private async void SearchOrder_Click(object sender, RoutedEventArgs e)
        {
            DateTime? dateBefore = date_before.SelectedDate;
            DateTime? dateAfter = date_after.SelectedDate;

            if (dateBefore.HasValue && dateAfter.HasValue)
            {
                List<OrderDTO> _result = await ManageOrderViewModel.SearchOrder(dateBefore.Value, dateAfter.Value);
                if (_result.Count > 0)
                {
                    ObservableCollection<OrderDTO> orderSearchList = new ObservableCollection<OrderDTO>(_result);
                    OrderComboBox.ItemsSource = orderSearchList;
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
        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void previousButton_Click(Object sender, RoutedEventArgs e)
        {

        }
        private void ResetInputData()
        {
            first_name.Text = "";
            last_name.Text = "";
            total_money.Text = "";
            delivery_date.SelectedDate = null;
            payment_status.IsChecked = false;
        }
    }
}