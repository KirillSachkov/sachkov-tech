using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Files.Contracts;
using SachkovTech.Files.Contracts.Converters;

namespace SachkovTech.Files.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddFilesPresentation(this IServiceCollection services)
    {
        services.AddScoped<IFilesContracts, FilesContracts>();
        services.AddScoped<IFormFileConverter, FormFileConverter>();

        return services;
    }
}