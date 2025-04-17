using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagement.Shared.Kernel;

public static class DependencyInjection
{
    public static IServiceCollection RegisterKernel(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
