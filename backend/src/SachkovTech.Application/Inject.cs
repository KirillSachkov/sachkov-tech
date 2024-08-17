using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Application.Modules.CreateModule;

namespace SachkovTech.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateModuleHandler>();

        return services;
    }
}