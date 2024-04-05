using PropertyChanged;
using Shoping.Business.OrderDetailServices;
using Shoping.Business.OrderServices;
using Shoping.Business.ProductServices;
using Shoping.Data_Access.DTOs;
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
        public ObservableCollection<ChartItemDTO> LstChartItems { get; set; }
        private int Step;
        public ReportViewModel(IOrderBusiness iOrderBusiness, IOrderDetailBusiness iOrderDetailBusiness, IProductBusiness iProductBusiness)
        {
            IOrderBusiness = iOrderBusiness;
            IOrderDetailBusiness = iOrderDetailBusiness;
            IProductBusiness = iProductBusiness;
        }

        public async Task GetBestSellingProductsInRange(DateTime from, DateTime to)
        {
            var lstOrderDetails = await IOrderDetailBusiness.GetOrderDetailsInRange(from, to);
            var lkOrderDetails = lstOrderDetails.ToLookup(c => c.ProductID);
            var lstProductIDs = lkOrderDetails.Select(c => c.Key).ToList();
            var lstProducts = await IProductBusiness.GetListProductsByRecID(lstProductIDs);

            var lstChartItemFromProducts = new List<ChartItemDTO>();
            foreach (var productID in lstProductIDs)
            {
                var soldQuantity = lkOrderDetails[productID].Sum(x => x.Quantity);
                var product = lstProducts.FirstOrDefault(x => x.ProductID == productID);
                lstChartItemFromProducts.Add(new ChartItemDTO
                {
                    ColumnName = product.Name,
                    Quantity = (int)soldQuantity,
                });
            }
            LstChartItems = new ObservableCollection<ChartItemDTO>(lstChartItemFromProducts);
        }
    }
}
