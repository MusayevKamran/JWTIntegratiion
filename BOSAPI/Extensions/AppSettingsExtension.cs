using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOSAPI.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BOSAPI.Extensions
{
    public static class AppSettingsExtension
    {
        public static IServiceCollection RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettingsConfiguration>(configuration.GetSection("AppSettings"));

            return services;
        }
    }
}
