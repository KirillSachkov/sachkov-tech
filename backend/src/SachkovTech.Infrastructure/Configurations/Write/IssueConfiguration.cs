using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Application.Dtos;
using SachkovTech.Domain.IssueManagement.Entities;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Infrastructure.Configurations.Write;

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

        builder.HasOne(i => i.ParentIssue)
            .WithMany(p => p.SubIssues)
            .HasForeignKey("parent_id")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(i => i.Files)
            .HasConversion(
                files => JsonSerializer.Serialize(
                    files.Select(f => new IssueFileDto
                    {
                        PathToStorage = f.PathToStorage.Path
                    }),
                    JsonSerializerOptions.Default),
                
                json => JsonSerializer.Deserialize<List<IssueFileDto>>(json, JsonSerializerOptions.Default)!
                    .Select(dto => new IssueFile(FilePath.Create(dto.PathToStorage).Value))
                    .ToList(),
                
                new ValueComparer<IReadOnlyList<IssueFile>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => (IReadOnlyList<IssueFile>)c.ToList()))
            .HasColumnType("jsonb")
            .HasColumnName("files");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}