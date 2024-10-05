using SachkovTech.Files.Application.Modles;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Presentation.Modules.Responses
{
    public record UploadFilesToIssueResponse(IEnumerable<Guid> FileIds, ErrorList UploadErrors, int UploadFilesCount, int NotUploadedFilesCount)
    {
        public static UploadFilesToIssueResponse MapFromUploadFilesResult(UploadFilesResult uploadFilesResult)
        {
            var responsefileIds = uploadFilesResult.UploadedFileIds.Select(id => id.Value);

            return new UploadFilesToIssueResponse(responsefileIds, uploadFilesResult.UploadErrors, uploadFilesResult.UploadFilesCount, uploadFilesResult.NotUploadedFilesCount);
        }
    }
}
