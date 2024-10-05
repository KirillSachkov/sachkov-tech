using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Infrastructure.Database;
using SachkovTech.Files.Infrastructure.Interfaces;
using SachkovTech.Files.Infrastructure.Options;
using SachkovTech.Files.Infrastructure.Providers;

namespace SachkovTech.Files.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFilesInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCommands();
            services.AddDatabase();
            services.AddMinio(configuration);

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddScoped<FilesWriteDbContext>();

            services.AddScoped<IFilesRepository, FilesRepository>();

            return services;
        }

        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes
                    .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

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
}
