using LiveCharts;
using Shoping.Presentation.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Wpf;

namespace Shoping.Presentation.Control
{
    public partial class StatisticUserControl : UserControl
    {
        DateTime startDate = DateTime.Today, endDate = DateTime.Today;
        int year = 0, choose = 0;
        public StatisticViewModel StatisticViewModel { get; set; }
        public DrawChartModel DrawChartModel { get; set; }
        public StatisticUserControl()
        {
            InitializeComponent();
            StatisticViewModel = new StatisticViewModel(App.iOrderBusiness, App.iOrderDetailBusiness, App.iProductBusiness);
            DrawChartModel = new DrawChartModel();
            StatisticCombobox.SelectedIndex = 0;
            MyChart.Visibility = Visibility.Collapsed;
        }
        private void StatisticCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatisticCombobox.SelectedIndex == 0)
            {
                DatePicker.Visibility = Visibility.Visible;
                if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
                {
                    startDate = (DateTime)StartDate.SelectedDate;
                    endDate = (DateTime)EndDate.SelectedDate;
                }
            } else
            {
                DatePicker.Visibility = Visibility.Collapsed;
            }

            if (StatisticCombobox.SelectedIndex == 1 || StatisticCombobox.SelectedIndex == 2)
            {
                InputYear.Visibility = Visibility.Visible;
                if (Regex.IsMatch(txtYear.Text, @"^\d+$"))
                {
                    year = int.Parse(txtYear.Text);
                }
            }
            else
            {
                InputYear.Visibility = Visibility.Collapsed;
            }

            choose = StatisticCombobox.SelectedIndex;
        }
        private async void RevenueButton_Click(object sender, RoutedEventArgs e)
        {
            MyChart.Visibility = Visibility.Visible;

            var information = await StatisticViewModel.GetRevenueAndSpendingInform(choose, startDate, endDate, year);
            List<int> profits = [];

            for (int i = 0; i < information.Item1.Count; ++i)
            {
                int profit = information.Item1[i] - information.Item2[i];
                profits.Add(profit);
            }

            MyChart = DrawChartModel.DrawDoubleLineChartByTime(information.Item1, profits, "Revenue", "Profit", information.Item3, information.Item4);
        }

        private async void SaleVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            MyChart.Visibility = Visibility.Visible;

            var information = await StatisticViewModel.GetSaleVolumeInform(choose, startDate, endDate, year);

            MyChart = DrawChartModel.DrawColumnChartByTime();
        }
    }
}
