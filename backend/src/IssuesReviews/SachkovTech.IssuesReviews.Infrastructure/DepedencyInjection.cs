using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.IssuesReviews.Application;
using SachkovTech.IssuesReviews.Infrastructure.DbContexts;
using SachkovTech.IssuesReviews.Infrastructure.Repositories;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Infrastructure;

public static class DepedencyInjection
{
    public static IServiceCollection AddIssuesReviewsInfrastructure(
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
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.IssuesReviews);
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IIssueReviewRepository, IssueReviewRepository>();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<IssueReviewsWriteDbContext>();
        services.AddScoped<IReadDbContext, IssueReviewsReadDbContext>();

        return services;
    }
}