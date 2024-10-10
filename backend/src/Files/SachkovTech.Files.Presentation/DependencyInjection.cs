using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Files.Contracts;

namespace SachkovTech.Files.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFilesPresentation(this IServiceCollection services)
        {
            services.AddScoped<IFilesContracts, FilesContracts>();

            return services;
        }
    }
}
