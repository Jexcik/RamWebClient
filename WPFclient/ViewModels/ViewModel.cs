using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFclient.ViewModels.Base;

namespace WPFclient.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            MainWindowVM = new MainWindowVM();

            ExternalServicesVM = new ExternalServicesVM();
        }

        public MainWindowVM MainWindowVM { get; set; }

        public ExternalServicesVM ExternalServicesVM { get; set; }
    }
}
