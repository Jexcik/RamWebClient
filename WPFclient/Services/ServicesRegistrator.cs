using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFclient.Services.Interfaces;

namespace WPFclient.Services
{
    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileChangeDataService, FileChangeDataService>();

            return services;
        }
    }
}
