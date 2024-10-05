using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Files.Application.Commands;
using SachkovTech.Files.Application.Modles;
using SachkovTech.Issues.Domain.Entities;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Commands.UploadFilesToIssue;

public class UploadFilesToIssueHandler : ICommandHandler<UploadFilesResult, UploadFilesToIssueCommand>
{
    private const string BUCKET_NAME = "files";

    private readonly IUploadFilesHandler _uploadFilesHandler;
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UploadFilesToIssueCommand> _validator;
    private readonly ILogger<UploadFilesToIssueHandler> _logger;

    public UploadFilesToIssueHandler(
        IUploadFilesHandler uploadFilesHandler,
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        IValidator<UploadFilesToIssueCommand> validator,
        ILogger<UploadFilesToIssueHandler> logger)
    {
        _uploadFilesHandler = uploadFilesHandler;
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<UploadFilesResult, ErrorList>> Handle(
        UploadFilesToIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var moduleResult = await _modulesRepository
            .GetById(ModuleId.Create(command.ModuleId), cancellationToken);

        if (moduleResult.IsFailure)
            return moduleResult.Error.ToErrorList();

        var issueId = IssueId.Create(command.IssueId);

        var issueResult = moduleResult.Value.GetIssueById(issueId);
        if (issueResult.IsFailure)
            return issueResult.Error.ToErrorList();

        List<UploadFileData> filesData = [];
        foreach (var file in command.Files)
        {
            var fileData = new UploadFileData(file.Content, BUCKET_NAME, file.FileName);

            filesData.Add(fileData);
        }

        var uploadFilesCommand = new UploadFilesCommand(nameof(Issue), command.IssueId, filesData);

        var uploadFileResult = await _uploadFilesHandler.Handle(uploadFilesCommand, cancellationToken);

        //var filePathsResult = await _uploadFilesHandler.UploadFiles(filesData, cancellationToken);
        if (uploadFileResult.IsFailure)
        {
            return uploadFileResult.Error;
        }

        //// уставовить всё в тру

        //issueResult.Value.UpdateFiles(issueFiles);

        //await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Success uploaded files to issue - {id}", issueResult.Value.Id.Value);

        return uploadFileResult;
    }
}