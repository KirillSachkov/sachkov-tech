using CSharpFunctionalExtensions;
using SachkovTech.Application.FileProvider;
using SachkovTech.Application.Providers;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Modules.AddIssue;

public class AddIssueHandler
{
    private readonly IFileProvider _fileProvider;

    public AddIssueHandler(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    public async Task<Result<string, Error>> Handle(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        return await _fileProvider.UploadFile(fileData, cancellationToken);
    }
}