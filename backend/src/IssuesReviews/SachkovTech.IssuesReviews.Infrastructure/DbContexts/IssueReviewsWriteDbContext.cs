using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.IssuesReviews.Domain;

namespace SachkovTech.IssuesReviews.Infrastructure.DbContexts;

public class IssueReviewsWriteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<IssueReview> IssueReviews => Set<IssueReview>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("issues-reviews");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IssueReviewsWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        
        modelBuilder.HasDefaultSchema("issues_reviews");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}