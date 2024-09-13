using CSharpFunctionalExtensions;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Files;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);
}