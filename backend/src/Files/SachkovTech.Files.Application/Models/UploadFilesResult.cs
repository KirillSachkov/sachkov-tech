using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Infrastructure.Models
{
    public record UploadFilesResult(string BucketName, string FileName, FilePath FilePath, FileSize FileSize);
}
