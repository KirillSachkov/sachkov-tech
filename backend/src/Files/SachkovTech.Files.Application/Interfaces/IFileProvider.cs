using CSharpFunctionalExtensions;
using SachkovTech.Files.Contracts.Dtos;
using SachkovTech.Files.Infrastructure.Models;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Application.Interfaces
{
    public interface IFileProvider
    {
        IAsyncEnumerable<Result<UploadFilesResult, Error>> UploadFiles(IEnumerable<UploadFileData> filesData, CancellationToken cancellationToken = default);

        Task<UnitResult<Error>> RemoveFile(FileLocation filesLocation, CancellationToken cancellationToken = default);
    }
}