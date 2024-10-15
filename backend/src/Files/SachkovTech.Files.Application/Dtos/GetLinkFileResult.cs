using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Application.Dtos
{
    public record GetLinkFileResult(FilePath FilePath, string Link);
}
