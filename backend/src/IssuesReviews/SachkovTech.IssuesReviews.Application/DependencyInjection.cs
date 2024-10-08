using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssuesReviews.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIssuesReviewsApplication(this IServiceCollection services)
    {
        services.AddScoped<IIssueReviewRepository, IssueReviewRepository>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        var assembly = typeof(DependencyInjection).Assembly;

        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandlerWithResult<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}