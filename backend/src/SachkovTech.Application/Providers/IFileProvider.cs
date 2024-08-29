using CSharpFunctionalExtensions;
using SachkovTech.Application.FileProvider;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(
        FileData fileData, CancellationToken cancellationToken = default);
}