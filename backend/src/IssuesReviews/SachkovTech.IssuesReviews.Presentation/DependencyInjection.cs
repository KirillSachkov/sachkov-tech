using Microsoft.Extensions.DependencyInjection;
using SachkovTech.IssuesReviews.Contracts;

namespace SachkovTech.IssuesReviews.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddIssuesReviewsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IIssuesReviewsContract, IssuesReviewsContract>();

        return services;
    }
}