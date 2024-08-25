using SachkovTech.API.Extensions;
using SachkovTech.API.Middlewares;
using SachkovTech.API.Validation;
using SachkovTech.Application;
using SachkovTech.Infrastructure;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
   .WriteTo.Console()
   .WriteTo.Debug()
   .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                ?? throw new ArgumentNullException("Seq"))
   .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
   .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
   .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
   .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilog();

builder.Services
    .AddInfrastructure()
    .AddApplication();

builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();