using SachkovTech.Files.Contracts.Dtos;

namespace SachkovTech.Files.Contracts.Requests;

public record UploadFilesRequest(string OwnerTypeName, Guid OwnerId, IEnumerable<UploadFileDto> Files);