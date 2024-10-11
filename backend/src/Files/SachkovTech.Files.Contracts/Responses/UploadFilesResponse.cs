using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Contracts.Responses;

public record UploadFilesResponse(IEnumerable<FileId> UploadedFileIds, int UploadFilesCount, int NotUploadedFilesCount);