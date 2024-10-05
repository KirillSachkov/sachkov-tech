using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Application.Modles
{
    public record FileLocation(string BucketName, FilePath FilePath);
}
