using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.IssueSolving.Domain.Entities;
using SachkovTech.IssueSolving.Domain.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssueSolving.Infrastructure.Configurations.Write
{
    public class UserIssueConfiguration : IEntityTypeConfiguration<UserIssue>
    {
        public void Configure(EntityTypeBuilder<UserIssue> builder)
        {
            builder.ToTable("user_issues");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasConversion(
                    push => push.Value,
                    pull => UserIssueId.Create(pull));

            builder.ComplexProperty(u => u.UserId, pb =>
            {
                pb.Property(a => a.Value)
                    .IsRequired()
                    .HasColumnName("user_id");
            });

            builder.ComplexProperty(u => u.UserId, pb =>
            {
                pb.Property(a => a.Value)
                    .IsRequired()
                    .HasColumnName("issue_id");
            });

            builder.Property(u => u.Status)
                .IsRequired()
                .HasConversion(
                    push => push.ToString(),
                    pull => Enum.Parse<IssueStatus>(pull)
                );

            builder.Property(u => u.StartDateOfExecution)
                .HasConversion(
                    push => push.ToUniversalTime(),
                    pull => pull.ToLocalTime()
                );

            builder.Property(u => u.EndDateOfExecution)
                .HasConversion(
                    push => push.ToUniversalTime(),
                    pull => pull.ToLocalTime()
                );

            builder.ComplexProperty(u => u.Attempts, pb =>
            {
                pb.Property(a => a.Value)
                    .IsRequired()
                    .HasColumnName("attempts");
            });


            //builder.Property(u => u.PullRequestUrl)
            //.HasConversion(
            //push => push.Value,
            //pull => PullRequestUrl.Create(pull).Value);

            builder.ComplexProperty(u => u.PullRequestUrl, pb =>
            {
                pb.Property(p => p.Value)
                    .IsRequired()
                    .HasColumnName("pull_request_url");
            });

        }
    }
}
