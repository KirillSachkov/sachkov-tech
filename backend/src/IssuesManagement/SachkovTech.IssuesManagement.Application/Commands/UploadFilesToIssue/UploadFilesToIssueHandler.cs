// using CSharpFunctionalExtensions;
// using FluentValidation;
// using Microsoft.Extensions.Logging;
// using SachkovTech.Core;
// using SachkovTech.Core.Abstractions;
// using SachkovTech.Core.Extensions;
// using SachkovTech.Core.Messaging;
// using SachkovTech.SharedKernel;
// using SachkovTech.SharedKernel.ValueObjects.Ids;
// using FileInfo = SachkovTech.Core.FileInfo;
//
// namespace SachkovTech.IssuesManagement.Application.Commands.UploadFilesToIssue;
//
// public class UploadFilesToIssueHandler : ICommandHandler<Guid, UploadFilesToIssueCommand>
// {
//     private const string BUCKET_NAME = "files";
//
//     private readonly IFileProvider _fileProvider;
//     private readonly IModulesRepository _modulesRepository;
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IValidator<UploadFilesToIssueCommand> _validator;
//     private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
//     private readonly ILogger<UploadFilesToIssueHandler> _logger;
//
//     public UploadFilesToIssueHandler(
//         IFileProvider fileProvider,
//         IModulesRepository modulesRepository,
//         IUnitOfWork unitOfWork,
//         IValidator<UploadFilesToIssueCommand> validator,
//         IMessageQueue<IEnumerable<FileInfo>> messageQueue,
//         ILogger<UploadFilesToIssueHandler> logger)
//     {
//         _fileProvider = fileProvider;
//         _modulesRepository = modulesRepository;
//         _unitOfWork = unitOfWork;
//         _validator = validator;
//         _messageQueue = messageQueue;
//         _logger = logger;
//     }
//
//     public async Task<Result<Guid, ErrorList>> Handle(
//         UploadFilesToIssueCommand command,
//         CancellationToken cancellationToken = default)
//     {
//         var validationResult = await _validator.ValidateAsync(command, cancellationToken);
//         if (validationResult.IsValid == false)
//         {
//             return validationResult.ToList();
//         }
//
//         var moduleResult = await _modulesRepository
//             .GetById(ModuleId.Create(command.ModuleId), cancellationToken);
//
//         if (moduleResult.IsFailure)
//             return moduleResult.Error.ToErrorList();
//
//         var issueId = IssueId.Create(command.IssueId);
//
//         var issueResult = moduleResult.Value.GetIssueById(issueId);
//         if (issueResult.IsFailure)
//             return issueResult.Error.ToErrorList();
//
//         List<FileData> filesData = [];
//         foreach (var file in command.Files)
//         {
//             var extension = Path.GetExtension(file.FileName);
//
//             var filePath = FilePath.Create("");
//             if (filePath.IsFailure)
//                 return filePath.Error.ToErrorList();
//
//             // var fileData = new FileData(file.Content, new FileInfo(filePath.Value, BUCKET_NAME));
//
//             // filesData.Add(fileData);
//         }
//
//         var filePathsResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
//         if (filePathsResult.IsFailure)
//         {
//             await _messageQueue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);
//
//             return filePathsResult.Error.ToErrorList();
//         }
//
//         // var issueFiles = filePathsResult.Value
//         //     .Select(f => new IssueFile(f))
//         //     .ToList();
//
//         // уставовить всё в тру
//
//         // issueResult.Value.UpdateFiles(issueFiles);
//
//         await _unitOfWork.SaveChanges(cancellationToken);
//
//         _logger.LogInformation("Success uploaded files to issue - {id}", issueResult.Value.Id.Value);
//
//         return issueResult.Value.Id.Value;
//     }
// }