using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.IssuesReviews.Domain;
using SachkovTech.IssuesReviews.Domain.Enums;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Infrastructure.Configurations.Write;

public class IssueReviewConfiguration : IEntityTypeConfiguration<IssueReview>
{
    public void Configure(EntityTypeBuilder<IssueReview> builder)
    {
        builder.ToTable("issue_reviews");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => IssueReviewId.Create(value));

        builder.ComplexProperty(i => i.UserIssueId, ub =>
        {
            ub.Property(i => i.Value)
                .HasColumnName("user_issue_id")
                .IsRequired();
        });

        builder.ComplexProperty(i => i.UserId, ub =>
        {
            ub.Property(i => i.Value)
                .HasColumnName("user_id")
                .IsRequired();
        });

        builder.Property(i => i.ReviewerId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));


        builder.Property(i => i.IssueReviewStatus)
            .HasConversion(
                status => status.ToString(),
                value => (IssueReviewStatus)Enum.Parse(typeof(IssueReviewStatus), value))
            .HasColumnName("issue_review_status")
            .IsRequired();

        builder.HasMany(c => c.Comments)
            .WithOne(c => c.IssueReview)
            .HasForeignKey("issue_review_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(i => i.ReviewStartedTime)
            .HasColumnName("review_started_time")
            .IsRequired();

        builder.Property(i => i.IssueTakenTime)
            .HasColumnName("issue_taken_time")
            .IsRequired(false);

        builder.Property(i => i.IssueApprovedTime)
            .HasColumnName("issue_approved_time")
            .IsRequired(false);

        builder.ComplexProperty(i => i.PullRequestUrl, pb =>
        {
            pb.Property(p => p.Value)
                .HasMaxLength(Constants.Default.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("pull_request_url")
                .IsRequired();
        });
    }
}