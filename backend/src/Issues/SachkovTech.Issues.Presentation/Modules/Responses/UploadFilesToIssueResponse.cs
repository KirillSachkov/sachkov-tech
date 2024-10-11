using SachkovTech.Files.Contracts.Responses;

namespace SachkovTech.Issues.Presentation.Modules.Responses;

public record UploadFilesToIssueResponse(IEnumerable<Guid> FileIds, int UploadFilesCount, int NotUploadedFilesCount)
{
    public static UploadFilesToIssueResponse MapFromUploadFilesResult(UploadFilesResponse uploadFilesResponse)
    {
        var responsefileIds = uploadFilesResponse.UploadedFileIds.Select(id => id.Value);

        return new UploadFilesToIssueResponse(responsefileIds, uploadFilesResponse.UploadFilesCount, uploadFilesResponse.NotUploadedFilesCount);
    }
}