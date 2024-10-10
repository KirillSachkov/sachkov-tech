using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Issues.Contracts;

namespace SachkovTech.Issues.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddIssuesPresentation(this IServiceCollection services)
    {
        services.AddScoped<IIssuesContract, IssuesContract>();

        return services;
    }
}