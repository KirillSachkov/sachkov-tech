using CSharpFunctionalExtensions;
using SachkovTech.Files.Application.Commands.UploadFiles;
using SachkovTech.Files.Application.Interfaces;
using SachkovTech.Files.Application.Modles;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Application
{
    internal class FilesContract : IFilesContract
    {
        private readonly IUploadFilesHandler _uploadFilesHandler;

        public FilesContract(IUploadFilesHandler uploadFilesHandler)
        {
            _uploadFilesHandler = uploadFilesHandler;
        }

        public async Task<Result<UploadFilesResult, ErrorList>> UploadFiles(string ownerTypeName, Guid ownerId, IEnumerable<UploadFileData> Files, CancellationToken cancellationToken = default)
        {
            var command = new UploadFilesCommand(ownerTypeName, ownerId, Files);

            return await _uploadFilesHandler.Handle(command, cancellationToken);
        }
    }
}
