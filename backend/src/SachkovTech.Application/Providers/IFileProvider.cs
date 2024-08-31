using CSharpFunctionalExtensions;
using SachkovTech.Application.FileProvider;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Providers;

public interface IFileProvider
{
    Task<UnitResult<Error>> UploadFiles(
        FileData fileData,
        CancellationToken cancellationToken = default);
}