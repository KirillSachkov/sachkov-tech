using Microsoft.AspNetCore.Authorization;
using SachkovTech.Accounts.Infrastructure;
using SachkovTech.Files.Application;
using SachkovTech.Files.Infrastructure;
using SachkovTech.Files.Presentation;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Application;
using SachkovTech.Issues.Infrastructure;
using SachkovTech.Issues.Presentation;
using SachkovTech.IssueSolving.Application;
using SachkovTech.IssueSolving.Infrastructure;
using SachkovTech.IssuesReviews.Application;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SachkovTech.Core.Options;
using System.Text;
using SachkovTech.Accounts.Application;
using SachkovTech.Accounts.Presentation;
using SachkovTech.IssuesReviews.Presentation;

namespace SachkovTech.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAccountsInfrastructure(configuration);
        services.AddAccountsApplication();
        services.AddAccountsPresentation();

        return services;
    }

    public static IServiceCollection AddIssuesReviewsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIssuesReviewsApplication();
        services.AddIssuesReviewsPresentation();

        return services;
    }

    public static IServiceCollection AddFilesModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFilesApplication();
        services.AddFilesInfrastructure(configuration);
        services.AddFilesPresentation();

        return services;
    }

    public static IServiceCollection AddIssuesModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIssuesApplication();
        services.AddIssuesInfrastructure(configuration);
        services.AddIssuesPresentation();

        return services;
    }

    public static IServiceCollection AddIssueSolvingModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIssueSolvingApplication();
        services.AddIssueSolvingInfrastructure(configuration);

        return services;
    }


    public static IServiceCollection AddAuthServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing jwt configuration");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddAuthorization();

        return services;
    }


    public static IServiceCollection AddLogging(
        this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(configuration.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq"))
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

        services.AddSerilog();

        return services;
    }
}