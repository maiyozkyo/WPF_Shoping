using LiveCharts;
using LiveCharts.Wpf;
using Shoping.Data_Access.DTOs;
using Shoping.Presentation.View;
using Shoping.Presentation.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace Shoping.Presentation.Control
{
    public partial class StatisticUserControl : UserControl
    {
        DateTime startDate = DateTime.Today, endDate = DateTime.Today.AddDays(1);
        int year = -1;
        int choose = 0;
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
            } else
            {
                DatePicker.Visibility = Visibility.Collapsed;
            }

            if (StatisticCombobox.SelectedIndex == 1 || StatisticCombobox.SelectedIndex == 2)
            {
                InputYear.Visibility = Visibility.Visible;
            }
            else
            {
                InputYear.Visibility = Visibility.Collapsed;
            }

            choose = StatisticCombobox.SelectedIndex;
        }
        private void InitializeChart()
        {
            MyChart.Visibility = Visibility.Visible;
            MyChart.Series.Clear();
            MyChart.AxisX.Clear();
            MyChart.AxisY.Clear();
            if (StartDate.SelectedDate.HasValue && EndDate.SelectedDate.HasValue)
            {
                startDate = StartDate.SelectedDate.Value;
                endDate = EndDate.SelectedDate.Value.AddDays(1);
            }
            if (Regex.IsMatch(txtYear.Text, @"^\d+$"))
            {
                year = int.Parse(txtYear.Text);
            } else { year = -1; }
        }
        private void UpdateChart(CartesianChart chart)
        {
            MyChart.Series = chart.Series;
            MyChart.AxisX = chart.AxisX;
            MyChart.AxisY = chart.AxisY;
        }
        private async void RevenueButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeChart();

            var information = await StatisticViewModel.GetRevenueAndSpendingInform(choose, startDate, endDate, year);
            List<int> profits = [];

            for (int i = 0; i < information.Item1.Count; ++i)
            {
                int profit = information.Item1[i] - information.Item2[i];
                profits.Add(profit);
            }

            var chart = DrawChartModel.DrawDoubleLineChartByTime(information.Item1, profits, "Revenue", "Profit", information.Item3, information.Item4, "USD");

            UpdateChart(chart);
        }

        private async void SaleVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeChart();

            var information = await StatisticViewModel.GetSaleVolumeInform(choose, startDate, endDate, year);

            var chart = DrawChartModel.DrawMultipleColumnChartByTime(information.Item1, information.Item2, information.Item3, "Quantity");

            UpdateChart(chart);
        }

        private async void BestSellingProducts_Click(object sender, RoutedEventArgs e)
        {
            InitializeChart();

            var information = await StatisticViewModel.GetSaleVolumeInform(choose, startDate, endDate, year);
            List<ChartItemDTO> bestSaleProducts = [];
            foreach(var productsByTime in information.Item1)
            {
                ChartItemDTO bestSaleProduct = productsByTime.OrderByDescending(x => x.Quantity).First();
                bestSaleProducts.Add(bestSaleProduct);
            }

            var chart = DrawChartModel.DrawColumnChartByTime(bestSaleProducts, information.Item2, information.Item3, "Quantity");

            UpdateChart(chart);
        }

    }
}
