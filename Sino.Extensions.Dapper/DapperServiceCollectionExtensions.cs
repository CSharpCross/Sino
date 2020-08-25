using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Dapper
{
    public static class DapperServiceCollectionExtensions
    {
        public static IServiceCollection AddDapper(this IServiceCollection services, string writeConnectionString, string readConnectionString)
        {
            services.AddSingleton<IDapperConfiguration>(new DapperConfiguration
            {
                WriteConnectionString = writeConnectionString,
                ReadConnectionString = readConnectionString
            });
            return services;
        }
    }
}
