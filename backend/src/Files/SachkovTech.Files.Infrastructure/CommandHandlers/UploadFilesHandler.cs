using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Files.Application.Commands;
using SachkovTech.Files.Application.Modles;
using SachkovTech.Files.Domain;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.Files.Infrastructure.Database;
using SachkovTech.Files.Infrastructure.Interfaces;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Infrastructure.CommandHandlers
{
    internal class UploadFilesHandler : IUploadFilesHandler
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<UploadFilesHandler> _logger;
        private readonly FilesWriteDbContext _dbContext;

        public UploadFilesHandler(IFileProvider fileProvider, ILogger<UploadFilesHandler> logger, FilesWriteDbContext dbContext)
        {
            _fileProvider = fileProvider;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<UploadFilesResult, ErrorList>> Handle(UploadFilesCommand command, CancellationToken cancellationToken = default)
        {
            List<FileId> fileIds = new();
            List<Error> errors = new();

            var uploadAsyncEnum = _fileProvider.UploadFiles(command.Files, cancellationToken);

            await foreach (var uploadFileResult in uploadAsyncEnum)
            {
                if (uploadFileResult.IsSuccess)
                {
                    var fileResult = uploadFileResult.Value;

                    var fileId = FileId.NewFileId();
                    var fileName = FileName.Create(fileResult.FileName).Value;
                    var ownerId = command.ownerId;
                    var storagePath = fileResult.FilePath;
                    var isUploaded = true;
                    var fileSize = FileSize.Create(fileResult.FileSize).Value;
                    var mimeType = MimeType.Png;
                    var fileType = FileType.Image;
                    var ownerType = OwnerType.Create(command.ownerTypeName).Value;

                    try
                    {
                        var fileData = new FileData(
                            fileId,
                            fileName,
                            ownerId,
                            storagePath,
                            isUploaded,
                            fileSize,
                            mimeType,
                            fileType,
                            ownerType);

                        await _dbContext.FileData.AddAsync(fileData, cancellationToken);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        fileIds.Add(fileData.Id);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex,
                            "Failed to save file data with id \"{fileId}\" to the database.", fileId.Value.ToString());

                        errors.Add(Error.Failure("file.upload", "Fail to upload file in minio"));

                        var fileLocation = new FileLocation(fileResult.BucketName, storagePath);

                        await _fileProvider.RemoveFile(fileLocation, cancellationToken);
                    }
                }
                else
                {
                    errors.Add(uploadFileResult.Error);
                }
            }

            if(fileIds.Count > 0)
            {
                var result = new UploadFilesResult(fileIds, errors, fileIds.Count + errors.Count, errors.Count);

                return result;
            }
            
            return new ErrorList([Error.Failure("file.upload", "Fail to upload files in minio")]);
        }
    }
}
