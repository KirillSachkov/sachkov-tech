using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Domain.Modules;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Infrastructure.Configurations;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => IssueId.Create(value));

        builder.Property(i => i.LessonId)
            .IsRequired(false);

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.HasOne(i => i.ParentIssue)
            .WithMany(p => p.SubIssues)
            .HasForeignKey("parent_id")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(i => i.Details, ib =>
        {
            ib.ToJson();

            ib.OwnsMany(d => d.Files, fb =>
            {
                fb.Property(f => f.PathToStorage)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            });
        });
    }
}