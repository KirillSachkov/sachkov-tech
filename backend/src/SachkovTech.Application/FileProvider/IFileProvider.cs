using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.FileProvider;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);
}