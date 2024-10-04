using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Accounts.Application;
using SachkovTech.Accounts.Domain;
using SachkovTech.Core.Options;

namespace SachkovTech.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));

        services.RegisterIdentity();

        services.AddScoped<AuthorizationDbContext>();

        return services;
    }

    private static void RegisterIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();
    }
}