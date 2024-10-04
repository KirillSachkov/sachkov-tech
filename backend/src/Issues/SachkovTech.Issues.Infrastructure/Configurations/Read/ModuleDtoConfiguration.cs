using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Core.Dtos;

namespace SachkovTech.Issues.Infrastructure.Configurations.Read;

public class ModuleDtoConfiguration : IEntityTypeConfiguration<ModuleDto>
{
    public void Configure(EntityTypeBuilder<ModuleDto> builder)
    {
        builder.ToTable("modules");

        builder.HasKey(i => i.Id);

        builder.HasMany(i => i.Issues)
            .WithOne()
            .HasForeignKey(i => i.ModuleId);
    }
}