using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;

namespace SachkovTech.IssuesReviews.Infrastructure.Configurations.Read;

public class IssueReviewDtoConfiguration : IEntityTypeConfiguration<IssueReviewDto>
{
    public void Configure(EntityTypeBuilder<IssueReviewDto> builder)
    {
        builder.ToTable("issue_reviews");

        builder.HasKey(i => i.Id);

        builder.HasMany(i => i.Comments)
            .WithOne()
            .HasForeignKey(i => i.IssueReviewId);
    }
}