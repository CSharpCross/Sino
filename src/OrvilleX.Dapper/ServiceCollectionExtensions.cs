using Microsoft.Extensions.DependencyInjection;

namespace OrvilleX.Dapper
{
    public static class ServiceCollectionExtensions
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
