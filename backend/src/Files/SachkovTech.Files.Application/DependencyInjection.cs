using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Files.Application.Interfaces;

namespace SachkovTech.Files.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFilesApplication(this IServiceCollection services)
        {
            services.AddScoped<IFilesContract, FilesContract>();

            return services;
        }
    }
}
