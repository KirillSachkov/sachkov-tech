using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.IssuesReviews.Domain;
using SachkovTech.IssuesReviews.Domain.Entities;
using SachkovTech.IssuesReviews.Domain.Enums;
using SachkovTech.IssuesReviews.Domain.ValueObjects;
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

        builder.Property(i => i.UserIssueId)
            .HasConversion(
                id => id.Value,
                value => UserIssueId.Create(value));

        builder.Property(i => i.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(i => i.ReviewerId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .HasColumnName("reviewer_id")
            .IsRequired(false);

        builder.Property(i => i.IssueReviewStatus)
            .HasConversion(
                status => status.ToString(),
                value => (IssueReviewStatus)Enum.Parse(typeof(IssueReviewStatus), value))
            .HasColumnName("issue_review_status")
            .IsRequired();

        builder.Property(i => i.Comments)
            .ValueObjectsCollectionJsonConversion(
                comment => new CommentDto(
                    comment.UserId.Value,
                    comment.Message.Value,
                    comment.CreatedAt,
                    comment.Id.Value),
                dto => Comment.Create(
                    UserId.Create(dto.UserId),
                    Message.Create(dto.Message).Value).Value)
            .HasColumnName("comments")
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

        builder.ComplexProperty(i => i.PullRequestLink, pb =>
        {
            pb.Property(pb => pb.Value)
                .HasMaxLength(Constants.Default.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("pull_request_link")
                .IsRequired();
        });

    }
}