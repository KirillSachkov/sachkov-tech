using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Accounts.Contracts;

namespace SachkovTech.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAccountsContract, AccountsContract>();

        return services;
    }
}