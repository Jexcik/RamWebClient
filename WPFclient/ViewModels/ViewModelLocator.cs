using Microsoft.Extensions.DependencyInjection;
using WPFclient.ViewModels.TabItem;
using WPFclient.Views;

namespace WPFclient.ViewModels
{
    public class ViewModelLocator
    {
        public ExternalServTabVM ExternalServTabVM => App.Host.Services.GetRequiredService<ExternalServTabVM>();
        public UpdateCenterTabVM UpdateCenterTabVM => App.Host.Services.GetRequiredService<UpdateCenterTabVM>();

    }
}
