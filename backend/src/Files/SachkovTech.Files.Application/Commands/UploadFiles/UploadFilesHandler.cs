using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Application.Commands.UploadFiles;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Contracts.Dtos;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.Files.Domain;
using SachkovTech.Files.Infrastructure.Models;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Infrastructure.CommandHandlers
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
                    var fileResult = uploadFileResult.Value;

                    FileId fileId = FileId.NewFileId();

                    var fileName = FileName.Create(fileResult.FileName).Value;
                    var fileSize = FileSize.Create(fileResult.FileSize).Value;
                    var mimeType = MimeType.Parse(fileResult.FileName).Value;
                    var fileType = FileType.Parse(fileResult.FileName).Value;
                    var ownerType = OwnerType.Create(command.OwnerTypeName).Value;

                    var fileData = new FileData(
                        fileId,
                        fileName,
                        command.OwnerId,
                        fileResult.FilePath,
                        true,
                        fileSize,
                        mimeType,
                        fileType,
                        ownerType);

                    var saveFileResult = await _filesRepository.Add(fileData, cancellationToken);

                    if (saveFileResult.IsSuccess)
                    {
                        fileIds.Add(fileId);
                    }
                    else
                    {
                        errors.Add(saveFileResult.Error);

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
                var result = new UploadFilesResponse(fileIds, fileIds.Count + errors.Count, errors.Count);

                return result;
            }
            
            return new ErrorList([Error.Failure("file.upload", "Fail to upload files in minio")]);
        }
    }
}
