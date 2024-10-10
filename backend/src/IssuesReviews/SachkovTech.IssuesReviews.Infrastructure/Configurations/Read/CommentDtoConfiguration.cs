using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;

namespace SachkovTech.IssuesReviews.Infrastructure.Configurations.Read;

public class CommentDtoConfiguration : IEntityTypeConfiguration<CommentDto>
{
    public void Configure(EntityTypeBuilder<CommentDto> builder)
    {
        builder.ToTable("issue_reviews");

        builder.HasKey(c => c.Id);
    }
}