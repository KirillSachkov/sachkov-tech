using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Application.Modules.AddIssue;
using SachkovTech.Application.Modules.Create;
using SachkovTech.Application.Modules.Delete;
using SachkovTech.Application.Modules.UpdateMainInfo;
using SachkovTech.Application.Modules.UploadFilesToIssue;

namespace SachkovTech.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateModuleHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<DeleteModuleHandler>();
        services.AddScoped<UploadFilesToIssueHandler>();
        services.AddScoped<AddIssueHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}