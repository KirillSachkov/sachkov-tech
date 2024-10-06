using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Application.Commands.UploadFiles;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Contracts.Dtos;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.Files.Domain;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Application.Commands
{
    public class UploadFilesHandler : ICommandHandler<UploadFilesResponse, UploadFilesCommand>
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

        public async Task<Result<UploadFilesResponse, ErrorList>> Handle(UploadFilesCommand command, CancellationToken cancellationToken = default)
        {
            List<FileId> fileIds = new();
            List<Error> errors = new();

            var uploadAsyncEnum = _fileProvider.UploadFiles(command.Files, cancellationToken);

            await foreach (var uploadFileResult in uploadAsyncEnum)
            {
                if (uploadFileResult.IsSuccess)
                {
                    var uploadResult = uploadFileResult.Value;

                    var saveFileResult = await SaveFile(uploadResult, command, cancellationToken);

                    if (saveFileResult.IsSuccess)
                        fileIds.Add(saveFileResult.Value);

                    else errors.Add(saveFileResult.Error);
                }
                else
                {
                    errors.Add(uploadFileResult.Error);
                }
            }

            if (fileIds.Count > 0)
            {
                var result = new UploadFilesResponse(fileIds, fileIds.Count + errors.Count, errors.Count);

                return result;
            }

            return new ErrorList([Error.Failure("file.upload", "Fail to upload files in minio")]);
        }



        private async Task<Result<FileId, Error>> SaveFile(
            UploadFilesResult uploadFileResult,
            UploadFilesCommand command,
            CancellationToken cancellationToken)
        {
            var fileData = CreateFileData(uploadFileResult, command);

            var saveFileResult = await _filesRepository.Add(fileData, cancellationToken);

            if (saveFileResult.IsSuccess)
            {
                return fileData.Id;
            }
            else
            {
                var fileLocation = new FileLocation(uploadFileResult.BucketName, uploadFileResult.FilePath);

                await _fileProvider.RemoveFile(fileLocation, cancellationToken);

                return saveFileResult.Error;
            }
        }

        private FileData CreateFileData(
            UploadFilesResult uploadFileResult,
            UploadFilesCommand command)
        {
            FileId fileId = FileId.NewFileId();

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
