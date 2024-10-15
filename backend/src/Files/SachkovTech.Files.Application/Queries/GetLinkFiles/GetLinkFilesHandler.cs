using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Domain.ValueObjects;

namespace SachkovTech.Files.Application.Queries.GetLinkFiles
{
    public class GetLinkFilesHandler : IQueryHandler<IEnumerable<FileLinkDto>, GetLinkFilesQuery>
    {
        private readonly IFileProvider _fileProvider;
        private readonly IFilesReadDbContext _readDbContext;

        public GetLinkFilesHandler(
            IFileProvider fileProvider,
            IFilesReadDbContext readDbContext)
        {
            _fileProvider = fileProvider;
            _readDbContext = readDbContext;
        }

        public async Task<IEnumerable<FileLinkDto>> Handle(
            GetLinkFilesQuery query,
            CancellationToken cancellationToken = default)
        {
            var files = await _readDbContext.Files.Where(f => query.FileIds.Contains(f.Id)).ToListAsync(cancellationToken);

            var getFileLinks = await _fileProvider.GetLinks(files.Select(f => FilePath.Create(f.StoragePath).Value));

            var results = from link in getFileLinks
                          let id = files.First(f => f.StoragePath == link.FilePath).Id
                          select new FileLinkDto(id, link.Link);

            return results.ToList();
        }
    }
}
