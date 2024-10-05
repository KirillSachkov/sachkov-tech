using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.Files.Application.Modles
{
    public record UploadFilesResult(IEnumerable<FileId> UploadedFileIds, ErrorList UploadErrors, int UploadFilesCount, int NotUploadedFilesCount);
}
