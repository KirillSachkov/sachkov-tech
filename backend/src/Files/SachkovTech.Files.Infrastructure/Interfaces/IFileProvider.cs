using CSharpFunctionalExtensions;
using SachkovTech.Files.Application.Modles;
using SachkovTech.Files.Infrastructure.Models;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Infrastructure.Interfaces
{
    internal interface IFileProvider
    {
        IAsyncEnumerable<Result<UploadFilesResponse, Error>> UploadFiles(IEnumerable<UploadFileData> filesData, CancellationToken cancellationToken = default);

        Task<UnitResult<Error>> RemoveFile(FileLocation filesLocation, CancellationToken cancellationToken = default);
    }
}