using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.Files.Domain;

namespace SachkovTech.Files.Infrastructure.Database;

internal class FilesWriteDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggeFactory;

    public FilesWriteDbContext(IConfiguration configuration, ILoggerFactory loggeFactory)
    {
        _configuration = configuration;
        _loggeFactory = loggeFactory;
    }

    public DbSet<FileData> FileData => Set<FileData>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(_loggeFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("files");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FilesWriteDbContext).Assembly,
            type => type.FullName?.Contains("Database.Configurations.Write") ?? false);
    }
}