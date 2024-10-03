using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SachkovTech.Domain.FilesManagement;
using SachkovTech.Domain.Shared.ValueObjects.Ids;
using SachkovTech.Infrastructure.Extensions;

namespace SachkovTech.Infrastructure.Configurations.Write;

public class FileDataConfiguration : IEntityTypeConfiguration<FileData>
{
    public void Configure(EntityTypeBuilder<FileData> builder)
    {
        builder.ToTable("files");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasConversion(
                id => id.Value,
                value => FileId.Create(value));

        builder.Property(f => f.IsUploaded);

        builder.Property(f => f.UploadDate);

        builder.Property(f => f.OwnerId);

        builder.ComplexProperty(f => f.OwnerType, ob =>
        {
            ob.Property(o => o.Value)
                .HasColumnName("owner_type");
        });

        builder.ComplexProperty(f => f.FileSize, lb =>
        {
            lb.Property(l => l.Value)
                .HasColumnName("file_size");
        });

        builder.ComplexProperty(f => f.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("name");
        });

        builder.ComplexProperty(f => f.FileType, fb =>
        {
            fb.Property(t => t.Value)
                .HasColumnName("file_type");
        });

        builder.ComplexProperty(f => f.MimeType, mb =>
        {
            mb.Property(t => t.Value)
                .HasColumnName("mime_type");
        });

        builder.ComplexProperty(f => f.StoragePath, sb =>
        {
            sb.Property(t => t.Value)
                .HasColumnName("storage_path");
        });

        builder.Property(i => i.Attributes)
            .ValueObjectsCollectionJsonConversion(
                attribute => attribute,
                attributeDto => attributeDto)
            .HasColumnName("attributes");
    }
}