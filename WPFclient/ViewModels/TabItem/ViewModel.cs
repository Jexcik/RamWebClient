using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFclient.ViewModels.Base;

namespace WPFclient.ViewModels.TabItem
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            MainWindowVM = new UpdateCenterTabVM();
        }

        public UpdateCenterTabVM MainWindowVM { get; set; }
    }
}
