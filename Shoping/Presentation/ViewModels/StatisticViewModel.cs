using Shoping.Business.OrderDetailServices;
using Shoping.Business.OrderServices;
using Shoping.Business.ProductServices;
using Shoping.Data_Access.DTOs;

namespace Shoping.Presentation.ViewModels
{
    public class StatisticViewModel(IOrderBusiness orderBusiness, IOrderDetailBusiness orderDetailBusiness, IProductBusiness productBusiness)
    {
        public IOrderBusiness OrderBusiness = orderBusiness;
        public IOrderDetailBusiness OrderDetailBusiness = orderDetailBusiness;
        public IProductBusiness ProductBusiness = productBusiness;
        string X_Title = "";
        List<string> X_Labels = [];

        private void Inittialize(int choose)
        {
            X_Labels.Clear();
            switch(choose)
            {
                case 0: X_Title = "Date";
                    break;
                case 1: X_Title = "Week";
                    for (int i = 0; i < 53; i++)
                    {
                        X_Labels.Add($"{i + 1}");
                    }
                    break;
                case 2: X_Title = "Month";
                    X_Labels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                    break;
                default: X_Title = "Year";
                    for (int i = 10; i >= 0; i--)
                    {
                        X_Labels.Add($"{DateTime.Today.Year - i}");
                    }
                    break;
            }
        }
        public async Task<(List<int>, List<int>, string, List<string>)> GetRevenueAndSpendingInform(int choose, DateTime startDate, DateTime endDate, int year)
        {
            List<int> revenues = [], spending = [];

            Inittialize(choose);

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
            }
            else if (choose == 2)
            {
                revenues = await OrderBusiness.GetRevenueByMonthAsync(year);
                spending = await ProductBusiness.GetSpendingByMonthAsync(year);
            }
            else
            {
                revenues = await OrderBusiness.GetRevenueByYearAsync();
                spending = await ProductBusiness.GetSpendingByYearAsync();
            }

            return (revenues, spending, X_Title, X_Labels);
        }
        
        public async Task<(List<List<ChartItemDTO>>, string, List<string>)> GetSaleVolumeInform(int choose, DateTime startDate, DateTime endDate, int year)
        {
            List<List<ChartItemDTO>> listProductsByTime = [];

            Inittialize(choose);

            if (choose == 0)
            {
                var productsAndSaleDates = await OrderDetailBusiness.GetSaleVolumnInDateRangeAsync(startDate, endDate);
                listProductsByTime = productsAndSaleDates.Item1;
                X_Labels = productsAndSaleDates.Item2;
            }
            else if (choose == 1)
            {
                listProductsByTime = await OrderDetailBusiness.GetSaleVolumnByWeekAsync(year);
            }
            else if (choose == 2)
            {
                listProductsByTime = await OrderDetailBusiness.GetSaleVolumnByMonthAsync(year);
            }
            else
            {
                listProductsByTime = await OrderDetailBusiness.GetSaleVolumnByYearAsync();
            }
   
            return (listProductsByTime, X_Title, X_Labels);
        }
    }
}
