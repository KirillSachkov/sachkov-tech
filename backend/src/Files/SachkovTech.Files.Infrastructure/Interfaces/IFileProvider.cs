using CSharpFunctionalExtensions;
using SachkovTech.Files.Domain.ValueObjects;
using SachkovTech.Files.Infrastructure.Modles;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Infrastructure.Interfaces
{
    internal interface IFileProvider
    {
        Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(IEnumerable<UploadFileData> filesData, CancellationToken cancellationToken = default);

        Task<UnitResult<Error>> RemoveFile(FileLocation filesLocation, CancellationToken cancellationToken = default);
    }
}