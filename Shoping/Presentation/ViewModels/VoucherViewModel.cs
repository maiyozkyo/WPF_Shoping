using PropertyChanged;
using Shoping.Business.VoucherServices;
using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class VoucherViewModel
    {
        public IVoucherBusiness VoucherBusiness { get; set; }
        public VoucherDTO VoucherDTO { get; set; }
        public string Code { get; set; }
        public BindingList<VoucherDTO> LstVouchers { get; set; }
        public VoucherViewModel(IVoucherBusiness iVoucherBusiness)
        {
            VoucherBusiness = iVoucherBusiness;
            VoucherDTO = new VoucherDTO
            {
                ExpiredOn = DateTime.Now.AddDays(2)
            };
        }

        public async Task<bool> AddUpdateVoucher()
        {
            VoucherDTO.Code = Code;
            var res = await VoucherBusiness.AddUpdateVoucher(VoucherDTO);
            if (res == true)
            {
                VoucherDTO = new VoucherDTO();
                Code = "";
            }
            return res;
        }

        public async Task GetVouchers()
        {
            var lstVouchers = await VoucherBusiness.GetVouchers(false);
            LstVouchers = new BindingList<VoucherDTO>(lstVouchers);
        }

        public async Task GenVoucherCode()
        {
            await Task.Delay(1);
        }
        
    }
}
