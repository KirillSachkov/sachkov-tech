using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Application.Queries
{
    public class GetLinkFilesHandler : IQueryHandler<Result<IEnumerable<FileLinkDto>, Error>, GetLinkFilesQuery>
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

        public async Task<Result<IEnumerable<FileLinkDto>, Error>> Handle(
            GetLinkFilesQuery query,
            CancellationToken cancellationToken = default)
        {

            var files = await _readDbContext.Files.Where(f => query.FileIds.Contains(f.Id)).ToListAsync(cancellationToken);

            var getFileLinks = await _fileProvider.GetLinks(files.Select(f => FilePath.Create(f.StoragePath).Value));

            if (getFileLinks.IsFailure)
                return getFileLinks.Error;

            var results = from link in getFileLinks.Value
                      let id = files.First(f => f.StoragePath == link.FilePath).Id
                      select new FileLinkDto(id, link.Link);

            return results.ToList();
        }
    }
}
