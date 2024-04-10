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
        private DateTime StartDate { get; }
        private DateTime EndDate { get; }

        public Report(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            ReportViewModel = new ReportViewModel(App.iOrderBusiness, App.iOrderDetailBusiness, App.iProductBusiness);
            DataContext = ReportViewModel;
            StartDate = startDate;
            EndDate = endDate;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ReportViewModel.GetBestSellingProductsInRange(StartDate, EndDate);
        }

    }
}
