using CSharpFunctionalExtensions;
using SachkovTech.Application.FileProvider;
using SachkovTech.Application.Providers;
using SachkovTech.Domain.IssueManagement.Entities;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Application.Modules.AddIssue;

public class AddIssueHandler
{
    private const string BUCKET_NAME = "files";

    private readonly IFileProvider _fileProvider;
    private readonly IModulesRepository _modulesRepository;

    public AddIssueHandler(
        IFileProvider fileProvider,
        IModulesRepository modulesRepository)
    {
        _fileProvider = fileProvider;
        _modulesRepository = modulesRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var moduleResult = await _modulesRepository
            .GetById(ModuleId.Create(command.ModuleId), cancellationToken);

        if (moduleResult.IsFailure)
            return moduleResult.Error;

        var issueId = IssueId.NewIssueId();
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        var lessonId = LessonId.Empty();

        List<FileContent> fileContents = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error;

            var fileContent = new FileContent(
                file.Content, filePath.Value.Path);

            fileContents.Add(fileContent);
        }

        var fileData = new FileData(fileContents, BUCKET_NAME);

        var uploadResult = await _fileProvider
            .UploadFiles(fileData, cancellationToken);

        if (uploadResult.IsFailure)
            return uploadResult.Error;

        var filePaths = command.Files
            .Select(f => FilePath.Create(Guid.NewGuid(), f.FileName).Value);

        var issueFiles = filePaths.Select(f => new IssueFile(f));

        var issue = new Issue(
            issueId,
            title,
            description,
            lessonId,
            null,
            new FilesList(issueFiles));

        moduleResult.Value.AddIssue(issue);

        await _modulesRepository.Save(moduleResult.Value, cancellationToken);

        return issue.Id.Value;
    }
}