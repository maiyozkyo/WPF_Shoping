using Shoping.Data_Access.DTOs;
using Shoping.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public ReportViewModel ReportViewModel { get; set; }

        public Report()
        {
            InitializeComponent();
            ReportViewModel = new ReportViewModel(App.iOrderBusiness, App.iOrderDetailBusiness, App.iProductBusiness);
            DataContext = ReportViewModel;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fromDate = DateTime.Now.AddDays(-7);
            var toDate = DateTime.Now;
            await ReportViewModel.GetBestSellingProductsInRange(fromDate, toDate);
        }
    }
}
