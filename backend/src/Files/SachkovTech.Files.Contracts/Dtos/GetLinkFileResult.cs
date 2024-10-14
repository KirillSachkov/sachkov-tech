using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Contracts.Dtos
{
    public record GetLinkFileResult(FilePath FilePath, string Link);
}
