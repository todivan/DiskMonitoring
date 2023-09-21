using Infrastructure;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IVolumesScanner, VolumesScanner>();
        services.AddSingleton<IVolumesScanner, VolumesScanner2>();

        return services;
    }
}
