using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFclient.ViewModels.TabItem;

namespace WPFclient.ViewModels
{
    public static class Registrator
    {
        public static IServiceCollection RegisterViewModel(this IServiceCollection services) 
        {
            services.AddSingleton<ExternalServTabVM>();
            services.AddSingleton<UpdateCenterTabVM>();
            services.AddSingleton<ExternalServicesVM>();

            return services;
        }
    }
}
