using SachkovTech.Core.Abstractions;

namespace SachkovTech.Files.Application.Queries.GetLinkFiles
{
    public record GetLinkFilesQuery(IEnumerable<Guid> FileIds) : IQuery;
}
