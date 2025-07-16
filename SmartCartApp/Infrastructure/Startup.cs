using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddUserSettings(this IServiceCollection services)
    {
        return services;
    }
}
