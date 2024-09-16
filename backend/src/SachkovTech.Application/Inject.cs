using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Application.Abstraction;
using SachkovTech.Application.IssueManagement.Commands.AddIssue;
using SachkovTech.Application.IssueManagement.Commands.Create;
using SachkovTech.Application.IssueManagement.Commands.Delete;
using SachkovTech.Application.IssueManagement.Commands.UpdateMainInfo;
using SachkovTech.Application.IssueManagement.Commands.UploadFilesToIssue;
using SachkovTech.Application.IssueManagement.Queries.GetModulesWithPagination;

namespace SachkovTech.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly);;

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}