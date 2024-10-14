namespace SachkovTech.Core.Dtos
{
    public class FileDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string StoragePath { get; init; } = string.Empty;

        public DateTime UploadDate { get; init; }

        public bool IsUploaded { get; init; }

        public long FileSize { get; init; }

        public string MimeType { get; init; } = string.Empty;

        public string FileType { get; init; } = string.Empty;

        public string OwnerType { get; init; } = string.Empty;

        public Guid OwnerId { get; init; }

        public IEnumerable<string> Attributes { get; init; } = new List<string>();
    }
}
