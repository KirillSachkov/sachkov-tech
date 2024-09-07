using SachkovTech.Application.Dtos;

namespace SachkovTech.Application.Modules.UploadFilesToIssue;

public record UploadFilesToIssueCommand(Guid ModuleId, Guid IssueId, IEnumerable<UploadFileDto> Files);