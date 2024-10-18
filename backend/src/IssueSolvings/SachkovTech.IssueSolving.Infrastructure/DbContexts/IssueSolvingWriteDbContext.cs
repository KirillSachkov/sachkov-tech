using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.IssueSolving.Domain.Entities;

namespace SachkovTech.IssueSolving.Infrastructure.DbContexts;

public class IssueSolvingWriteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<UserIssue> UserIssues => Set<UserIssue>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("issues-solving");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IssueSolvingWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}