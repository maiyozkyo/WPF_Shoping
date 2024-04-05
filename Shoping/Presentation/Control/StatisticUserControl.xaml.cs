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
        public StatisticViewModel StatisticViewModel { get; set; }
        public StatisticUserControl()
        {
            InitializeComponent();
            StatisticViewModel = new StatisticViewModel(App.iOrderBusiness, App.iOrderDetailBusiness, App.iProductBusiness);
            StatisticCombobox.SelectedIndex = 0;
            revenueAndProfitChart.Visibility = Visibility.Collapsed;
        }
        private void StatisticCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatisticCombobox.SelectedIndex == 0)
            {
                DatePicker.Visibility = Visibility.Visible;
                InputYear.Visibility = Visibility.Collapsed;
            }
            else if (StatisticCombobox.SelectedIndex == 1 || StatisticCombobox.SelectedIndex == 2)
            {
                DatePicker.Visibility = Visibility.Collapsed;
                InputYear.Visibility = Visibility.Visible;
            }
            else
            {
                DatePicker.Visibility = Visibility.Collapsed;
                InputYear.Visibility = Visibility.Collapsed;
            }
        }
        private async void RevenueButton_Click(object sender, RoutedEventArgs e)
        {
            revenueAndProfitChart.Visibility = Visibility.Visible;
            var startDate = DateTime.Today;
            var endDate = DateTime.Today;
            var year = 0;
            var choose = StatisticCombobox.SelectedIndex;
            if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                startDate = (DateTime)StartDate.SelectedDate;
                endDate = (DateTime)EndDate.SelectedDate;
            }
            if (Regex.IsMatch(txtYear.Text, @"^\d+$"))
            {
                year = int.Parse(txtYear.Text);
            }
            var information = await StatisticViewModel.GetRevenueAndSpendingInform(choose, startDate, endDate, year);
            List<int> profits = [];

            for (int i = 0; i < information.Item1.Count; ++i)
            {
                int profit = information.Item1[i] - information.Item2[i];
                profits.Add(profit);
            }

            revenueAndProfitChart.Series.Clear();
            revenueAndProfitChart.AxisX.Clear();
            revenueAndProfitChart.AxisY.Clear();

            revenueAndProfitChart.Series =
            [
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

            if (information.Item1.Count + information.Item2.Count == 0)
            {
                revenueAndProfitChart.AxisY.Add(new Axis()
                {
                    MaxValue = 10,
                    MinValue = 0,
                });
            }

            revenueAndProfitChart.AxisX.Add(new Axis()
            {
                Title = information.Item3,
                Labels = information.Item4,
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
            });
        }
    }
}
