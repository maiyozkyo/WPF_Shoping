using Shoping.Business.StatisticServices;
using LiveCharts;
using LiveCharts.Wpf;

namespace Shoping.Presentation.ViewModels
{
    public class StatisticViewModel
    {
        IStatisticBusiness StatisticServices;

        public async Task<int> DrawRevenueAndProfitDiagram(string choose, DateTime startDate, DateTime endDate)
        {
            if(choose == "From date to date")
            {
                var revenue = await StatisticServices.GetRevenueInDateRangeAsync(startDate, endDate);
            } else if (choose == "By month")
            {
                var revenue = await StatisticServices.GetRevenueByMonthAsync();
            } else
            {
                var revenue = await StatisticServices.GetRevenueByYearAsync();
            }
            return 0;
        }
    }
}
