using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Dtos;
using System.Text.Json;

namespace SachkovTech.Files.Infrastructure.Database.Configurations.Read
{
    internal class FileDtoDataConfiguration : IEntityTypeConfiguration<FileDto>
    {
        public void Configure(EntityTypeBuilder<FileDto> builder)
        {
            builder.ToTable("files");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Attributes)
                .HasConversion(
                    files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<string[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
