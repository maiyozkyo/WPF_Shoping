using LiveCharts;
using LiveCharts.Wpf;
using Shoping.Presentation.View;
using Shoping.Presentation.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace Shoping.Presentation.Control
{
    public partial class StatisticUserControl : UserControl
    {
        DateTime startDate = DateTime.Today, endDate = DateTime.Today;
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
            if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                startDate = (DateTime)StartDate.SelectedDate;
                endDate = (DateTime)EndDate.SelectedDate;
            }
            if (Regex.IsMatch(txtYear.Text, @"^\d+$"))
            {
                year = int.Parse(txtYear.Text);
            } else { year = -1; }
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

            //MyChart = DrawChartModel.DrawDoubleLineChartByTime(information.Item1, profits, "Revenue", "Profit", information.Item3, information.Item4);

            MyChart.Series = [
                    new LineSeries
                    {
                        Title = "Revenue",
                        Values = new ChartValues<int>(information.Item1),
                    },
                new LineSeries
                {
                    Title = "Profit",
                    Values = new ChartValues<int>(profits),
                }
            ];

            int max = information.Item1.Count != 0 ? information.Item1.Max() : 0;
            int min = profits.Count != 0 ? profits.Min() : 0;
            if(max != min)
            {
                MyChart.AxisY.Add(new Axis()
                {
                    MaxValue = max,
                    MinValue = min,
                });
            }
            else
            {
                MyChart.AxisY.Add(new Axis()
                {
                    MaxValue = max,
                });
            }

            MyChart.AxisX.Add(new Axis()
            {
                Title = information.Item3,
                Labels = information.Item4,
                Separator = new LiveCharts.Wpf.Separator() { Step = 1 },
            });
        }

        private async void SaleVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeChart();
            if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                startDate = (DateTime)StartDate.SelectedDate;
                endDate = (DateTime)EndDate.SelectedDate;
            }

            var information = await StatisticViewModel.GetSaleVolumeInform(choose, startDate, endDate, year);

            //MyChart = DrawChartModel.DrawColumnChartByTime();
        }

        private async void BtnBestSellingProducts_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate.SelectedDate.HasValue && EndDate.SelectedDate.HasValue)
            {
                var reportWindow = new Report(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value);
                if (reportWindow.ShowDialog() == true)
                {

                }

            }
            else
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và ngày kết thúc!");
            }
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
