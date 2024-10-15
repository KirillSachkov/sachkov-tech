using CSharpFunctionalExtensions;
using SachkovTech.Core.Dtos;
using SachkovTech.Files.Application.Commands.UploadFiles;
using SachkovTech.Files.Application.Queries.GetLinkFiles;
using SachkovTech.Files.Contracts;
using SachkovTech.Files.Contracts.Requests;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Presentation
{
    internal class FilesContracts : IFilesContracts
    {
        private readonly UploadFilesHandler _uploadFilesHandler;
        private readonly GetLinkFilesHandler _getLinkFilesHandler;

        public FilesContracts(UploadFilesHandler uploadFilesHandler, GetLinkFilesHandler getLinkFilesHandler)
        {
            _uploadFilesHandler = uploadFilesHandler;
            _getLinkFilesHandler = getLinkFilesHandler;
        }

        public async Task<Result<UploadFilesResponse, ErrorList>> UploadFiles(
            UploadFilesRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UploadFilesCommand(request.OwnerTypeName, request.OwnerId, request.Files);

            return await _uploadFilesHandler.Handle(command, cancellationToken);
        }

        public async Task<IEnumerable<FileLinkDto>> GetLinkFiles(
            IEnumerable<Guid> fileIds, CancellationToken cancellationToken = default)
        {
            var command = new GetLinkFilesQuery(fileIds);

            return await _getLinkFilesHandler.Handle(command, cancellationToken);
        }
    }
}