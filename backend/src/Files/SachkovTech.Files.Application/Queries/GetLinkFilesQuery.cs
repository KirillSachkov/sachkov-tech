using SachkovTech.Core.Abstractions;

namespace SachkovTech.Files.Application.Queries
{
    public record GetLinkFilesQuery(IEnumerable<Guid> FileIds) : IQuery;
}
