using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.IssueSolving.Application;
using SachkovTech.IssueSolving.Infrastructure.DbContexts;
using SachkovTech.IssueSolving.Infrastructure.Repositories;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssueSolving.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIssueSolvingInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts()
            .AddRepositories()
            .AddDatabase();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.IssueSolving);
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserIssueRepository, UserIssueRepository>();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<IssueSolvingWriteDbContext>();
        services.AddScoped<IReadDbContext, IssueSolvingReadDbContext>();

        return services;
    }

}