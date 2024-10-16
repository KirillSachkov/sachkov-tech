using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.Files.Contracts;
using SachkovTech.Files.Contracts.Dtos;
using SachkovTech.Files.Contracts.Requests;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.Issues.Domain.Entities;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Issues.Application.Commands.UploadFilesToIssue;

public class UploadFilesToIssueHandler : ICommandHandler<UploadFilesResponse, UploadFilesToIssueCommand>
{
    private const string BUCKET_NAME = "files";

    private readonly IFilesContracts _filesContracts;
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UploadFilesToIssueCommand> _validator;
    private readonly ILogger<UploadFilesToIssueHandler> _logger;

    public UploadFilesToIssueHandler(
        IFilesContracts filesContracts,
        IModulesRepository modulesRepository,
        [FromKeyedServices(Modules.Issues)] IUnitOfWork unitOfWork,
        IValidator<UploadFilesToIssueCommand> validator,
        ILogger<UploadFilesToIssueHandler> logger)
    {
        _filesContracts = filesContracts;
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<UploadFilesResponse, ErrorList>> Handle(
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

        var request = new UploadFilesRequest(nameof(Issue), command.IssueId, command.Files);

        var uploadFileResult = await _filesContracts.UploadFiles(request, cancellationToken);

        if (uploadFileResult.IsFailure)
        {
            return uploadFileResult.Error;
        }

        var issueFiles = issueResult.Value.Files.ToList();

        issueFiles.AddRange(uploadFileResult.Value.UploadedFileIds);

        issueResult.Value.UpdateFiles(issueFiles);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Success uploaded files to issue - {id}", issueResult.Value.Id.Value);

        return uploadFileResult;
    }
}