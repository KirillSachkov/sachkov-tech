using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.IssuesReviews.Domain;
using SachkovTech.IssuesReviews.Domain.Entities;
using SachkovTech.IssuesReviews.Domain.Enums;
using SachkovTech.IssuesReviews.Domain.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Infrastructure.Configurations.Read;

public class IssueReviewDtoConfiguration: IEntityTypeConfiguration<IssueReviewDto>
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