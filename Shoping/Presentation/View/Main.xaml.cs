using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shoping.Presentation.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public MainViewModel MainViewModel { get; set; }
        public Main()
        {
            InitializeComponent();
            MainViewModel = new MainViewModel(App.iOrderBusiness);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    var orderDTO = new OrderDTO
            //    {
            //        CustomerID = Guid.NewGuid(),
            //        Paid = i,
            //        Total = 1000,
            //    };
            //    await MainViewModel.AddUpdateOrder(orderDTO);
            //}
            var pageData = await MainViewModel.Paging(3, 20);
            var x = pageData.Data;
            foreach(var item in x)
            {

            }
            int a = 1;
        }
    }
}
