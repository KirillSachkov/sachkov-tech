using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using SachkovTech.Application.Database;
using SachkovTech.Application.Files;
using SachkovTech.Application.Messaging;
using SachkovTech.Application.Modules;
using SachkovTech.Infrastructure.BackgroundServices;
using SachkovTech.Infrastructure.MessageQueues;
using SachkovTech.Infrastructure.Options;
using SachkovTech.Infrastructure.Providers;
using SachkovTech.Infrastructure.Repositories;
using FileInfo = SachkovTech.Application.Files.FileInfo;

namespace SachkovTech.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IModulesRepository, ModulesRepository>();
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddMinio(configuration);

        services.AddHostedService<FilesCleanerBackgroundService>();

        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}