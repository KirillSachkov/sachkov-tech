using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Application.Dtos;

namespace SachkovTech.Infrastructure.Configurations.Read;

public class IssueDtoConfiguration : IEntityTypeConfiguration<IssueDto>
{
    public void Configure(EntityTypeBuilder<IssueDto> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);

        builder.HasOne<IssueDto>()
            .WithMany()
            .HasForeignKey(i => i.ParentId)
            .IsRequired(false);
    }
}