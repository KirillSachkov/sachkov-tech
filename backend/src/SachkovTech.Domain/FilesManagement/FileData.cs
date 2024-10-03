using System.Net.Http.Headers;
using System.Net.Mime;
using SachkovTech.Domain.FilesManagement.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Domain.FilesManagement;

public class FileData : CSharpFunctionalExtensions.Entity<FileId>
{
    private List<FileAttribute> _attributes = [];

    // ef core
    private FileData(FileId id) : base(id)
    {
    }

    public FileData(
        FileId id,
        FileName name,
        Guid ownerId,
        FilePath storagePath,
        bool isUploaded,
        FileSize fileSize,
        MimeType mimeType,
        FileType fileType,
        OwnerType ownerType)
        : base(id)
    {
        Name = name;
        OwnerId = ownerId;
        StoragePath = storagePath;
        UploadDate = DateTime.UtcNow;
        IsUploaded = isUploaded;
        FileSize = fileSize;
        MimeType = mimeType;
        FileType = fileType;
        OwnerType = ownerType;
    }

    public FileName Name { get; private set; }

    public FilePath StoragePath { get; private set; }

    public DateTime UploadDate { get; private set; }

    public bool IsUploaded { get; private set; }

    public FileSize FileSize { get; private set; }

    public MimeType MimeType { get; private set; }

    public FileType FileType { get; private set; }

    public OwnerType OwnerType { get; private set; }
    
    public Guid OwnerId { get; private set; }

    public IReadOnlyList<FileAttribute> Attributes => _attributes;
}