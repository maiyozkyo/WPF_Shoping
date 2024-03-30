using Shoping.Business.OrderDetailServices;
using Shoping.Business.OrderServices;
using Shoping.Business.ProductServices;

namespace Shoping.Presentation.ViewModels
{
    public class StatisticViewModel(IOrderBusiness orderBusiness, IOrderDetailBusiness orderDetailBusiness, IProductBusiness productBusiness)
    {
        public IOrderBusiness OrderBusiness = orderBusiness;
        public IOrderDetailBusiness OrderDetailBusiness = orderDetailBusiness;
        public IProductBusiness ProductBusiness = productBusiness;

        public async Task<Tuple<List<int>, string, List<string>>> GetRevenueAndProfitInform(int choose, DateTime startDate, DateTime endDate, int year)
        {
            List<int> revenues;
            string X_Title;
            List<string> X_Labels = [];

            if (choose == 0)
            {
                (revenues, X_Labels) = await OrderBusiness.GetRevenueInDateRangeAsync(startDate, endDate);
                X_Title = "Date";
            }
            else if (choose == 1)
            {
                revenues = await OrderBusiness.GetRevenueByWeekAsync(year);
                X_Title = "Week";
                for(int i = 0; i < 53; i++)
                {
                    X_Labels.Add($"{i + 1}");
                }
            }
            else if (choose == 2)
            {
                revenues = await OrderBusiness.GetRevenueByMonthAsync(year);
                X_Title = "Month";
                X_Labels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            }
            else
            {
                revenues = await OrderBusiness.GetRevenueByYearAsync();
                X_Title = "Year";
                for(int i = 10; i >= 0; i--)
                {
                    X_Labels.Add($"{ DateTime.Today.Year - i }");
                }
            }

            return new Tuple<List<int>, string, List<string>>(revenues, X_Title, X_Labels);
        }

        public async Task<List<int>> GetSpendingInform(int choose, DateTime startDate, DateTime endDate, int year)
        {
            List<int> spending;

            if (choose == 0)
            {
                spending = await ProductBusiness.GetSpendingInDateRangeAsync(startDate, endDate);
            }
            else if (choose == 1)
            {
                spending = await ProductBusiness.GetSpendingByWeekAsync(year);
            }
            else if (choose == 2)
            {
                spending = await ProductBusiness.GetSpendingByMonthAsync(year);
            }
            else
            {
                spending = await ProductBusiness.GetSpendingByYearAsync();
            }

            return spending;
        }
    }
}
