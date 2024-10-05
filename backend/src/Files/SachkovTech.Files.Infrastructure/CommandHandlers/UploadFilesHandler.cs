using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Files.Application.Commands.UploadFiles;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Application.Modles;
using SachkovTech.Files.Infrastructure.Interfaces;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Infrastructure.CommandHandlers
{
    internal class UploadFilesHandler : IUploadFilesHandler
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<UploadFilesHandler> _logger;
        private readonly IFilesRepository _filesRepository;

        public UploadFilesHandler(IFileProvider fileProvider, ILogger<UploadFilesHandler> logger, IFilesRepository filesRepository)
        {
            _fileProvider = fileProvider;
            _logger = logger;
            _filesRepository = filesRepository;
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

                    FileId fileId = FileId.NewFileId();

                    try
                    {
                        await _filesRepository.Add(fileId, fileResult.FileName, fileResult.FilePath, fileResult.FileSize, command.ownerId, command.ownerTypeName, cancellationToken);

                        fileIds.Add(fileId);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex,
                            "Failed to save file data with id \"{fileId}\" to the database.", fileId.Value.ToString());

                        errors.Add(Error.Failure("file.upload", "Fail to upload file in minio"));

                        var fileLocation = new FileLocation(fileResult.BucketName, fileResult.FilePath);

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
