using Shoping.Presentation.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using LiveCharts;
using LiveCharts.Wpf;


namespace Shoping.Presentation
{
    public partial class Statistic : Window
    {
        StatisticViewModel StatisticViewModel { get; set; }
        public Statistic()
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
            if(Regex.IsMatch(txtYear.Text, @"^\d+$"))
            {
                year = int.Parse(txtYear.Text);
            }
            var revenueInformation = await StatisticViewModel.GetRevenueAndProfitInform(choose, startDate, endDate, year);
            var spending = await StatisticViewModel.GetSpendingInform(choose, startDate, endDate, year);
            List<int> profits = [];
            for (int i = 0; i < revenueInformation.Item1.Count; ++i)
            {
                int profit = revenueInformation.Item1[i] - spending[i];
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
                    Values = new ChartValues<int>(revenueInformation.Item1),
                },
                new LineSeries
                {
                    Title = "Profit",
                    Values = new ChartValues<int>(profits),
                }
            ];

            revenueAndProfitChart.AxisY.Add(new Axis()
            {
                MaxValue = revenueInformation.Item1.Max(),
                MinValue = profits.Min(),
            });

            revenueAndProfitChart.AxisX.Add(new Axis()
            {
                Title = revenueInformation.Item2,
                Labels = revenueInformation.Item3,
                Separator = new LiveCharts.Wpf.Separator { Step = 1 },
            });
        }
    }
}
