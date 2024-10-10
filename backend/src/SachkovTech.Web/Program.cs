using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SachkovTech.Accounts.Application;
using SachkovTech.Web.Middlewares;
using SachkovTech.Accounts.Infrastructure;
using SachkovTech.Accounts.Presentation;
using SachkovTech.Core.Options;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Application;
using SachkovTech.Issues.Infrastructure;
using SachkovTech.Issues.Presentation;
using SachkovTech.Issues.Presentation.Issues;
using SachkovTech.Issues.Presentation.Modules;
using SachkovTech.IssuesReviews.Application;
using SachkovTech.IssueSolving.Application;
using SachkovTech.IssueSolving.Infrastructure;
using Serilog;
using Serilog.Events;
using SachkovTech.Files.Infrastructure;
using SachkovTech.Files.Application;
using SachkovTech.Files.Presentation;
using SachkovTech.IssuesReviews.Infrastructure;
using SachkovTech.Web.Extensions;
using LoggerConfigurationExtensions = SachkovTech.Web.Extensions.LoggerConfigurationExtensions;

var builder = WebApplication.CreateBuilder(args);


LoggerConfigurationExtensions.ConfigureLogging(builder);

SwaggerConfigurationExtensions.ConfigureSwagger(builder);

builder.Services.AddSerilog();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

AuthenticationConfigurationExtensions.ConfigureAuthentication(builder);

builder.Services.AddAuthorization();

builder.Services
    .AddAccountsApplication()
    .AddAccountsInfrastructure(builder.Configuration)
    
    .AddIssuesReviewsApplication()
    .AddIssuesReviewsInfrastructure(builder.Configuration)

    .AddFilesApplication()
    .AddFilesInfrastructure(builder.Configuration)
    .AddFilesPresentation()

    .AddIssuesApplication()
    .AddIssuesManagementInfrastructure(builder.Configuration)
    .AddIssuesPresentation()
  
    .AddIssueSolvingApplication()
    .AddIssueSolvingInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();

await accountsSeeder.SeedAsync();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();