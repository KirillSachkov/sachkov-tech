using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddScoped<CreateModuleHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<DeleteModuleHandler>();
        services.AddScoped<UploadFilesToIssueHandler>();
        services.AddScoped<AddIssueHandler>();
        
        services.AddScoped<GetIssuesWithPaginationHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}