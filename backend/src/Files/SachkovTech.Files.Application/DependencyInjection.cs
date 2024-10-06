using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.Files.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFilesApplication(this IServiceCollection services)
        {
            services.AddCommands();
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
    }
}
