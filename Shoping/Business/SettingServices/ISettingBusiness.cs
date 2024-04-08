using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Business.SettingServices
{
    public interface ISettingBusiness
    {
        Task<bool> Backup(string filePath);
        Task<bool> Restore(List<string> lstFiles);

    }
}
