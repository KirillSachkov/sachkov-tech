using CSharpFunctionalExtensions;
using SachkovTech.Files.Application.Commands;
using SachkovTech.Files.Application.Modles;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Application.Interfaces
{
    public interface IFilesContract
    {
        public Task<Result<UploadFilesResult, ErrorList>> UploadFiles(string ownerTypeName, Guid ownerId, IEnumerable<UploadFileData> Files, CancellationToken cancellationToken = default);
    }
}
