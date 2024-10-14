using CSharpFunctionalExtensions;
using SachkovTech.Files.Contracts.Dtos;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Application.Interfaces;

public interface IFileProvider
{
    IAsyncEnumerable<Result<UploadFilesResult, Error>> UploadFiles(IEnumerable<UploadFileData> filesData, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<GetLinkFileResult>, Error>> GetLinks(IEnumerable<FilePath> filePaths, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveFile(FilePath filePath, CancellationToken cancellationToken = default);
}