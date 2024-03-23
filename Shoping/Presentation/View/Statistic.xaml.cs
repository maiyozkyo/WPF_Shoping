using Shoping.Presentation.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace Shoping.Presentation
{
    public partial class Statistic : Window
    {
        public StatisticViewModel StatisticViewModel { get; set; }
        public Statistic()
        {
            InitializeComponent();
            StatisticViewModel = new StatisticViewModel();
        }

        private void StatisticCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StatisticCombobox.SelectedItem.ToString() == "From date to date")
            {
                DatePicker.Visibility = Visibility.Visible;
            } else
            {
                DatePicker.Visibility = Visibility.Collapsed;
            }
        }

        private async void RevenueButton_Click(object sender, RoutedEventArgs e)
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Today;
            if(StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                startDate = (DateTime)StartDate.SelectedDate;
                endDate = (DateTime)EndDate.SelectedDate;
            }
            var isSuccess = await StatisticViewModel.DrawRevenueAndProfitDiagram(StatisticCombobox.SelectedIndex.ToString(), startDate, endDate);
        }
    }
}
