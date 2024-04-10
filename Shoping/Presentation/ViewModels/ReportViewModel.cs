using LiveCharts;
using LiveCharts.Wpf;
using PropertyChanged;
using Shoping.Business.OrderDetailServices;
using Shoping.Business.OrderServices;
using Shoping.Business.ProductServices;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ReportViewModel
    {
        public IOrderBusiness IOrderBusiness { get; set; }
        public IOrderDetailBusiness IOrderDetailBusiness { get; set; }
        public IProductBusiness IProductBusiness { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        private int Step;
        public ReportViewModel(IOrderBusiness iOrderBusiness, IOrderDetailBusiness iOrderDetailBusiness, IProductBusiness iProductBusiness)
        {
            IOrderBusiness = iOrderBusiness;
            IOrderDetailBusiness = iOrderDetailBusiness;
            IProductBusiness = iProductBusiness;
            SeriesCollection = new SeriesCollection();
        }

        public async Task GetBestSellingProductsInRange(DateTime from, DateTime to)
        {
            var listOrderDetails = await IOrderDetailBusiness.GetOrderDetailsInRange(from, to);
            var lkOrderDetails = listOrderDetails.ToLookup(c => c.ProductID);
            var listProductIDs = lkOrderDetails.Select(c => c.Key).ToList();
            var listProducts = await IProductBusiness.GetListProductsByRecID(listProductIDs);

            var listChartItemFromProducts = new List<ChartItemDTO>();
            foreach (var productID in listProductIDs)
            {
                var soldQuantity = lkOrderDetails[productID].Sum(x => x.Quantity);
                var product = lstProducts.FirstOrDefault(x => x.ProductID == productID);
                if (product != null)
                {
                    lstChartItemFromProducts.Add(new ChartItemDTO
                    {
                        ColumnName = product.Name,
                        Quantity = (int)soldQuantity,
                    });
                }
            }

            lstChartItemFromProducts = lstChartItemFromProducts.OrderByDescending(x => x.Quantity).ToList();
            foreach(var item in lstChartItemFromProducts)
            {
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = item.ColumnName,
                    Values = new ChartValues<int> { item.Quantity },
                });
            }
            Labels = lstChartItemFromProducts.Select(c => c.ColumnName).ToList();
        }
    }
}
