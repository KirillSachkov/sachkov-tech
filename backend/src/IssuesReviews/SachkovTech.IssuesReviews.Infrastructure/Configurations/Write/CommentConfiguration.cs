using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.IssuesReviews.Domain.Entities;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Infrastructure.Configurations.Write;

public class CommentConfiguration: IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CommentId.Create(value));
        
        builder.Property(c => c.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value))
            .HasColumnName("user_id");

        builder.ComplexProperty(c => c.Message, cb =>
        {
            cb.Property(m => m.Value)
                .HasMaxLength(Constants.Default.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("message")
                .IsRequired();
        });
        
        builder.Property(c => c.CreatedAt)
            .HasColumnType("create_at")
            .IsRequired();
    }
}