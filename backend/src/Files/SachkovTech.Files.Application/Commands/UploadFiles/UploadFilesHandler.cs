using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Contracts.Dtos;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.Files.Domain;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Application.Commands.UploadFiles
{
    public class UploadFilesHandler : ICommandHandler<UploadFilesResponse, UploadFilesCommand>
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<UploadFilesHandler> _logger;
        private readonly IFilesRepository _filesRepository;

        public UploadFilesHandler(IFileProvider fileProvider, ILogger<UploadFilesHandler> logger,
            IFilesRepository filesRepository)
        {
            _fileProvider = fileProvider;
            _logger = logger;
            _filesRepository = filesRepository;
        }

        public async Task<Result<UploadFilesResponse, ErrorList>> Handle(UploadFilesCommand command,
            CancellationToken cancellationToken = default)
        {
            List<FileId> fileIds = [];
            List<Error> errors = [];

            var uploadAsyncEnum = _fileProvider.UploadFiles(command.Files, cancellationToken);

            await foreach (var uploadFileResult in uploadAsyncEnum)
            {
                if (uploadFileResult.IsSuccess)
                {
                    var uploadResult = uploadFileResult.Value;

                    var saveFileResult = await SaveFile(uploadResult, command, cancellationToken);

                    if (saveFileResult.IsSuccess)
                        fileIds.Add(saveFileResult.Value);
                    else
                        errors.Add(saveFileResult.Error);
                }
                else
                {
                    errors.Add(uploadFileResult.Error);
                }
            }

            if (fileIds.Count <= 0)
                return new ErrorList([Error.Failure("file.upload", "Fail to upload files in minio")]);

            var result = new UploadFilesResponse(fileIds, fileIds.Count + errors.Count, errors.Count);

            return result;
        }


        private async Task<Result<FileId, Error>> SaveFile(
            UploadFilesResult uploadFileResult,
            UploadFilesCommand command,
            CancellationToken cancellationToken)
        {
            var fileData = CreateFileData(uploadFileResult, command);

            var saveFileResult = await _filesRepository.Add(fileData, cancellationToken);

            if (saveFileResult.IsSuccess)
                return fileData.Id;

            var fileLocation = new FileLocation(uploadFileResult.BucketName, uploadFileResult.FilePath);

            await _fileProvider.RemoveFile(fileLocation, cancellationToken);

            return saveFileResult.Error;
        }

        private FileData CreateFileData(
            UploadFilesResult uploadFileResult,
            UploadFilesCommand command)
        {
            var fileId = FileId.NewFileId();

            var fileName = FileName.Create(uploadFileResult.FileName).Value;
            var fileSize = FileSize.Create(uploadFileResult.FileSize).Value;
            var mimeType = MimeType.Parse(uploadFileResult.FileName).Value;
            var fileType = FileType.Parse(uploadFileResult.FileName).Value;
            var ownerType = OwnerType.Create(command.OwnerTypeName).Value;

            return new FileData(
                fileId,
                fileName,
                command.OwnerId,
                uploadFileResult.FilePath,
                true,
                fileSize,
                mimeType,
                fileType,
                ownerType);
        }
    }
}