using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Infrastructure.Models
{
    public record UploadFilesResponse(string BucketName, string FileName, FilePath FilePath, FileSize FileSize);
}
