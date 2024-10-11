using CSharpFunctionalExtensions;
using SachkovTech.Files.Contracts.Requests;
using SachkovTech.Files.Contracts.Responses;
using SachkovTech.SharedKernel;

namespace SachkovTech.Files.Contracts;

public interface IFilesContracts
{
    public Task<Result<UploadFilesResponse, ErrorList>> UploadFiles(UploadFilesRequest request, CancellationToken cancellationToken = default);
}