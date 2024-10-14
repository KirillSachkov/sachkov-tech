using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application;
using SachkovTech.Issues.Infrastructure.BackgroundServices;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.Issues.Infrastructure.Repositories;
using SachkovTech.Issues.Infrastructure.Services;

namespace SachkovTech.Issues.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIssuesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts()
            .AddRepositories()
            .AddDatabase()
            .AddHostedServices()
            .AddServices();

        return services;   
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(SharedKernel.ContextNames.Issues);

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IModulesRepository, ModulesRepository>();
        
        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();

        return services;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<DeleteExpiredIssuesBackgroundService>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<DeleteExpiredIssuesService>();

        return services;
    }
}