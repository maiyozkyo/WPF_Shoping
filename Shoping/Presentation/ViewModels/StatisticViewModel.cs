using Shoping.Business.OrderDetailServices;
using Shoping.Business.OrderServices;
using Shoping.Business.ProductServices;
using System.Linq;
using System.Windows.Controls.DataVisualization;

namespace Shoping.Presentation.ViewModels
{
    public class StatisticViewModel(IOrderBusiness orderBusiness, IOrderDetailBusiness orderDetailBusiness, IProductBusiness productBusiness)
    {
        public IOrderBusiness OrderBusiness = orderBusiness;
        public IOrderDetailBusiness OrderDetailBusiness = orderDetailBusiness;
        public IProductBusiness ProductBusiness = productBusiness;

        public async Task<Tuple<List<int>, List<int>, string, List<string>>> GetRevenueAndSpendingInform(int choose, DateTime startDate, DateTime endDate, int year)
        {
            List<int> revenues = [], spending = [];
            string X_Title;
            List<string> X_Labels = [];

            if (choose == 0)
            {
                List<(int, int, string)> revenueAndSpendingByDate = [];
                var revenueByDateRange = await OrderBusiness.GetRevenueInDateRangeAsync(startDate, endDate);
                var spendingByDateRange = await ProductBusiness.GetSpendingInDateRangeAsync(startDate, endDate);
                
                for(int i = 0; i < revenueByDateRange.Item1.Count; ++i)
                {
                    revenueAndSpendingByDate.Add((revenueByDateRange.Item1[i], 0, revenueByDateRange.Item2[i]));
                }
                for (int i = 0; i < spendingByDateRange.Item1.Count; ++i)
                {
                    var index = revenueAndSpendingByDate.FindIndex(x => x.Item3 == spendingByDateRange.Item2[i]);
                    if (index != -1)
                    {
                        var information = revenueAndSpendingByDate[index];
                        revenueAndSpendingByDate[index] = (information.Item1, spendingByDateRange.Item1[i], information.Item3);
                    } else
                    {
                        revenueAndSpendingByDate.Add((0, spendingByDateRange.Item1[i], spendingByDateRange.Item2[i]));
                    }
                }
                var orderedList = revenueAndSpendingByDate.OrderBy(x => x.Item3);

                X_Title = "Date";
                foreach (var tuple in orderedList)
                {
                    revenues.Add(tuple.Item1);
                    spending.Add(tuple.Item2);
                    X_Labels.Add(tuple.Item3);
                }
            }
            else if (choose == 1)
            {
                revenues = await OrderBusiness.GetRevenueByWeekAsync(year);
                spending = await ProductBusiness.GetSpendingByWeekAsync(year);
                X_Title = "Week";
                for (int i = 0; i < 53; i++)
                {
                    X_Labels.Add($"{i + 1}");
                }
            }
            else if (choose == 2)
            {
                revenues = await OrderBusiness.GetRevenueByMonthAsync(year);
                spending = await ProductBusiness.GetSpendingByMonthAsync(year);
                X_Title = "Month";
                X_Labels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            }
            else
            {
                revenues = await OrderBusiness.GetRevenueByYearAsync();
                spending = await ProductBusiness.GetSpendingByYearAsync();
                X_Title = "Year";
                for (int i = 10; i >= 0; i--)
                {
                    X_Labels.Add($"{DateTime.Today.Year - i}");
                }
            }

            return new Tuple<List<int>, List<int>, string, List<string>>(revenues, spending, X_Title, X_Labels);
        }
    }
}
