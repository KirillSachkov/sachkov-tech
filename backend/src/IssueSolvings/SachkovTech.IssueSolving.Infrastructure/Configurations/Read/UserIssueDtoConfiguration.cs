using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;

namespace SachkovTech.IssueSolving.Infrastructure.Configurations.Read;

public class UserIssueDtoConfiguration : IEntityTypeConfiguration<UserIssueDto>
{
    public void Configure(EntityTypeBuilder<UserIssueDto> builder)
    {
        builder.ToTable("user_issues");

        builder.HasKey(u => u.Id);
    }
}