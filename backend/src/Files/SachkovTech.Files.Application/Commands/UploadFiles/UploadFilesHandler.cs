using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Application.Dtos;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.Files.Domain;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;
using SachkovTech.Files.Contracts.Dtos;

namespace SachkovTech.Files.Application.Commands.UploadFiles;

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

        var uploadFiles = new List<UploadFileData>();

        foreach (var file in command.Files)
        {
            var mimeTypeResult = MimeType.Parse(file.FileName);
            if (mimeTypeResult.IsFailure)
                continue;

            var prefix = mimeTypeResult.Value.Value.ToLower();

            uploadFiles.Add(new UploadFileData(file.Content, command.OwnerTypeName.ToLower(), file.FileName, prefix));
        }

        var uploadAsyncEnum = _fileProvider.UploadFiles(uploadFiles, cancellationToken);

        await foreach (var uploadFileResult in uploadAsyncEnum)
        {
            if (uploadFileResult.IsFailure) continue;

            var uploadResult = uploadFileResult.Value;

            var saveFileResult = await SaveFile(uploadResult, command, cancellationToken);

            if (saveFileResult.IsSuccess)
                fileIds.Add(saveFileResult.Value);
        }

        if (fileIds.Count <= 0)
            return new ErrorList([Error.Failure("file.upload", "Fail to upload files in minio")]);

        var notUploadedFilesCount = command.Files.Count() - fileIds.Count;
        var result = new UploadFilesResponse(fileIds, fileIds.Count, notUploadedFilesCount);

        _logger.LogInformation("Files uploaded: {notUploadedFilesCount}", notUploadedFilesCount);

        return result;
    }


    private async Task<Result<FileId, ErrorList>> SaveFile(
        UploadFilesResult uploadFileResult,
        UploadFilesCommand command,
        CancellationToken cancellationToken)
    {
        var fileData = CreateFileData(uploadFileResult, command);
        if (fileData.IsFailure)
            return fileData.Error;

        var saveFileResult = await _filesRepository.Add(fileData.Value, cancellationToken);

        if (saveFileResult.IsSuccess)
            return fileData.Value.Id;

        await _fileProvider.RemoveFile(uploadFileResult.FilePath, cancellationToken);

        return saveFileResult.Error.ToErrorList();
    }

    private Result<FileData, ErrorList> CreateFileData(
        UploadFilesResult uploadFileResult,
        UploadFilesCommand command)
    {
        var fileId = FileId.NewFileId();

        var fileNameResult = FileName.Create(uploadFileResult.FileName);
        if (fileNameResult.IsFailure)
            return fileNameResult.Error.ToErrorList();

        var fileSizeResult = FileSize.Create(uploadFileResult.FileSize);
        if (fileSizeResult.IsFailure)
            return fileSizeResult.Error.ToErrorList();

        var mimeTypeResult = MimeType.Parse(uploadFileResult.FileName);
        if (mimeTypeResult.IsFailure)
            return mimeTypeResult.Error.ToErrorList();

        var fileTypeResult = FileType.Create(uploadFileResult.FileName);
        if (fileTypeResult.IsFailure)
            return fileTypeResult.Error.ToErrorList();

        var ownerTypeResult = OwnerType.Create(command.OwnerTypeName);
        if (ownerTypeResult.IsFailure)
            return ownerTypeResult.Error.ToErrorList();

        return new FileData(
            fileId,
            fileNameResult.Value,
            command.OwnerId,
            uploadFileResult.FilePath,
            true,
            fileSizeResult.Value,
            mimeTypeResult.Value,
            fileTypeResult.Value,
            ownerTypeResult.Value);
    }
}