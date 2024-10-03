// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Minio;
// using SachkovTech.Application.Files;
// using SachkovTech.Application.IssueManagement;
// using SachkovTech.Core;
// using SachkovTech.Core.Messaging;
// using SachkovTech.Infrastructure.BackgroundServices;
// using SachkovTech.Infrastructure.DbContexts;
// using SachkovTech.Infrastructure.Files;
// using SachkovTech.Infrastructure.MessageQueues;
// using SachkovTech.Infrastructure.Options;
// using SachkovTech.Infrastructure.Providers;
// using SachkovTech.Infrastructure.Repositories;
// using FileInfo = SachkovTech.Core.FileInfo;
//
// namespace SachkovTech.Infrastructure;
//
// public static class Inject
// {
//     public static IServiceCollection AddInfrastructure(
//         this IServiceCollection services, IConfiguration configuration)
//     {
//         services
//             .AddDbContexts()
//             .AddMinio(configuration)
//             .AddRepositories()
//             .AddDatabase()
//             .AddHostedServices()
//             .AddMessageQueues()
//             .AddServices();
//
//         return services;
//     }
//     
//     private static IServiceCollection AddServices(this IServiceCollection services)
//     {
//         services.AddScoped<IFilesCleanerService, FilesCleanerService>();
//
//         return services;
//     }
//     
//     private static IServiceCollection AddMessageQueues(this IServiceCollection services)
//     {
//         services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();
//
//         return services;
//     }
//     
//     private static IServiceCollection AddHostedServices(this IServiceCollection services)
//     {
//         services.AddHostedService<FilesCleanerBackgroundService>();
//
//         return services;
//     }
//
//     private static IServiceCollection AddDatabase(this IServiceCollection services)
//     {
//         services.AddScoped<IUnitOfWork, UnitOfWork>();
//         services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
//         
//         Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
//
//         return services;
//     }
//
//     private static IServiceCollection AddRepositories(this IServiceCollection services)
//     {
//         services.AddScoped<IModulesRepository, ModulesRepository>();
//
//         return services;
//     }
//
//     private static IServiceCollection AddDbContexts(this IServiceCollection services)
//     {
//         services.AddScoped<WriteDbContext>();
//         services.AddScoped<IReadDbContext, ReadDbContext>();
//
//         return services;
//     }
//
//     private static IServiceCollection AddMinio(
//         this IServiceCollection services, IConfiguration configuration)
//     {
//         services.Configure<MinioOptions>(
//             configuration.GetSection(MinioOptions.MINIO));
//
//         services.AddMinio(options =>
//         {
//             var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
//                                ?? throw new ApplicationException("Missing minio configuration");
//
//             options.WithEndpoint(minioOptions.Endpoint);
//
//             options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
//             options.WithSSL(minioOptions.WithSsl);
//         });
//
//         services.AddScoped<IFileProvider, MinioProvider>();
//
//         return services;
//     }
// }