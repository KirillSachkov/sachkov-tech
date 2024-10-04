using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Issues.Domain.Entities;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Infrastructure.Configurations.Write;

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

        builder.ComplexProperty(i => i.LessonId,
            lb =>
            {
                lb.Property(l => l.Value)
                    .IsRequired(false)
                    .HasColumnName("lesson_id");
            });
        
        builder.ComplexProperty(i => i.Experience,
            lb =>
            {
                lb.Property(l => l.Value)
                    .IsRequired()
                    .HasColumnName("experience");
            });

        builder.ComplexProperty(i => i.Position,
            lb =>
            {
                lb.Property(l => l.Value)
                    .IsRequired()
                    .HasColumnName("position");
            });

        builder.ComplexProperty(m => m.Title, tb =>
        {
            tb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Title.MAX_LENGTH)
                .HasColumnName("title");
        });

        builder.ComplexProperty(m => m.Description, tb =>
        {
            tb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Description.MAX_LENGTH)
                .HasColumnName("description");
        });
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}