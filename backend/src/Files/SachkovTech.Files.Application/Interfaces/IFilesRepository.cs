using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Application.Interfaces
{
    public interface IFilesRepository
    {
        public Task<FileId> Add(FileId fileId, string fileName, FilePath filePath, long fileSize, Guid ownerId, string ownerTypeName, CancellationToken cancellationToken);
    }
}
