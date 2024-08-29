using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Application.Modules.AddIssue;
using SachkovTech.Application.Modules.CreateModule;
using SachkovTech.Application.Modules.Delete;
using SachkovTech.Application.Modules.UpdateMainInfo;

namespace SachkovTech.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateModuleHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<DeleteModuleHandler>();
        services.AddScoped<AddIssueHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}