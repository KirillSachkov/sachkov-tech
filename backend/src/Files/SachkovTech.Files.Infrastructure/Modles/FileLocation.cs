using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Infrastructure.Modles
{
    public record FileLocation(FilePath FilePath, string BucketName);
}
