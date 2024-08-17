using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Application.Modules;
using SachkovTech.Infrastructure.Repositories;

namespace SachkovTech.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();

        services.AddScoped<IModulesRepository, ModulesRepository>();

        return services;
    }
}