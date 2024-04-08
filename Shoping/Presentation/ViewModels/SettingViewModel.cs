using PropertyChanged;
using Shoping.Business.SettingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoping.Presentation.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SettingViewModel
    {
        public ISettingBusiness SettingBusiness { get; set; }
        public SettingViewModel(ISettingBusiness settingBusiness)
        {
            SettingBusiness = settingBusiness;
        }

        public async Task<bool> Backup(string directory)
        {
            return await SettingBusiness.Backup(directory);
        }

        public async Task<bool> Restore(List<string> lstFiles)
        {
            return await SettingBusiness.Restore(lstFiles);
        }
    }
}
