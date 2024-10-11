using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Files.Contracts;


    public static IServiceCollection AddFilesPresentation(this IServiceCollection services)
    {
        services.AddScoped<IFilesContracts, FilesContracts>();

        return services;
    }
}