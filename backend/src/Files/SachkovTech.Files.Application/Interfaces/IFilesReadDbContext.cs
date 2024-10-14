using SachkovTech.Core.Dtos;

namespace SachkovTech.Files.Application.Interfaces
{
    public interface IFilesReadDbContext
    {
        public IQueryable<FileDto> Files { get; }
    }
}
