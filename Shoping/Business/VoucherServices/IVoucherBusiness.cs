using Shoping.Data_Access.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.VoucherServices
{
    public interface IVoucherBusiness
    {
        Task<bool> AddUpdateVoucher(VoucherDTO voucherDTO);
        Task<List<VoucherDTO>> GetVouchers(bool isValid);
    }
}
