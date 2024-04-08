using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoping.Data_Access.DTOs;
using Shoping.Data_Access.Models;
using Shoping.Data_Access.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.VoucherServices
{
    public class VoucherBusiness : BaseBusiness<Voucher>, IVoucherBusiness
    {
        public VoucherBusiness(string _dbName) : base(_dbName)
        {
        }

        public async Task<bool> AddUpdateVoucher(VoucherDTO voucherDTO)
        {
            var voucher = await Repository.GetOneAsync(x => x.Code == voucherDTO.Code);
            if (voucher == null)
            {
                voucher = new Voucher
                {
                    Code = voucherDTO.Code,
                };
                Repository.Add(voucher);
            }
            else
            {
                voucher.ModifiedOn = DateTime.Now;
                voucher.ModifiedBy = App.Auth.Email;
                Repository.Update(voucher);
            }

            voucher.IsPercent = voucherDTO.IsPercent;
            voucher.ExpiredOn = voucherDTO.ExpiredOn;
            voucher.Value = voucherDTO.Value;
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<VoucherDTO>> GetVouchers(bool IsValidVoucher)
        {
            var query = Repository.GetAsync(c => true);
            if (IsValidVoucher)
            {
                query = query.Where(x => x.ExpiredOn >= DateTime.Now);
            }
            var vouchers = await query.ToListAsync();
            var json = JsonConvert.SerializeObject(vouchers);
            var voucherDTOs = JsonConvert.DeserializeObject<List<VoucherDTO>>(json);
            return voucherDTOs;
        }
    }
}
