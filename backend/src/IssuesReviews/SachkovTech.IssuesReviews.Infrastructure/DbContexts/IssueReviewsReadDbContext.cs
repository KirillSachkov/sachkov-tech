using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Dtos;
using SachkovTech.IssuesReviews.Application;

namespace SachkovTech.IssuesReviews.Infrastructure.DbContexts;

public class IssueReviewsReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public IQueryable<IssueReviewDto> IssueReviewDtos => Set<IssueReviewDto>();
    public IQueryable<CommentDto> Comments => Set<CommentDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("issues-reviews");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IssueReviewsWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
}