using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Domain;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.Files.Infrastructure.Database;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Infrastructure
{
    internal class FilesRepository : IFilesRepository
    {
        private readonly FilesWriteDbContext _dbContext;

        public FilesRepository(FilesWriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FileId> Add(FileId fileId, string fileName, FilePath filePath, long fileSize, Guid ownerId, string ownerTypeName, CancellationToken cancellationToken)
        {
            var _fileName = FileName.Create(fileName).Value;
            var sownerId = ownerId;
            var storagePath = filePath;
            var isUploaded = true;
            var _fileSize = FileSize.Create(fileSize).Value;
            var mimeType = MimeType.Parse(fileName).Value;
            var fileType = FileType.Parse(fileName).Value;
            var ownerType = OwnerType.Create(ownerTypeName).Value;

            var fileData = new FileData(
                fileId,
                _fileName,
                ownerId,
                storagePath,
                isUploaded,
                _fileSize,
                mimeType,
                fileType,
                ownerType);

            await _dbContext.FileData.AddAsync(fileData, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return fileData.Id;
        }
    }
}
