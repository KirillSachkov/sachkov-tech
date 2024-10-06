using CSharpFunctionalExtensions;
using SachkovTech.Files.Application.Commands.UploadFiles;
using SachkovTech.Files.Contracts;
using SachkovTech.Files.Contracts.Requests;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.Files.Infrastructure.CommandHandlers;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Application
{
    internal class FilesContracts : IFilesContracts
    {
        private readonly UploadFilesHandler _uploadFilesHandler;

        public FilesContracts(UploadFilesHandler uploadFilesHandler)
        {
            _uploadFilesHandler = uploadFilesHandler;
        }

        public async Task<Result<UploadFilesResponse, ErrorList>> UploadFiles(UploadFilesRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UploadFilesCommand(request.OwnerTypeName, request.OwnerId, request.Files);

            return await _uploadFilesHandler.Handle(command, cancellationToken);
        }
    }
}
