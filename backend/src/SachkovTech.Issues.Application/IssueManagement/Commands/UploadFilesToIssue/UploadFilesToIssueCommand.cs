using SachkovTech.Core.Abstraction;
using SachkovTech.Core.Dtos;

namespace SachkovTech.Issues.Application.IssueManagement.Commands.UploadFilesToIssue;

public record UploadFilesToIssueCommand(Guid ModuleId, Guid IssueId, IEnumerable<UploadFileDto> Files) : ICommand;