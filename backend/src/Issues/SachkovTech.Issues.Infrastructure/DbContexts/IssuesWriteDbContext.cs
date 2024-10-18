using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Domain;

namespace SachkovTech.Issues.Infrastructure.DbContexts;

public class IssuesWriteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Module> Modules => Set<Module>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("issues");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IssuesWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}