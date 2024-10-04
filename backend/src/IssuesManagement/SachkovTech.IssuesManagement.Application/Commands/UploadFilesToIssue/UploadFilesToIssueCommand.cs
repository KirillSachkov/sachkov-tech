using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;

namespace SachkovTech.IssuesManagement.Application.Commands.UploadFilesToIssue;

public record UploadFilesToIssueCommand(Guid ModuleId, Guid IssueId, IEnumerable<UploadFileDto> Files) : ICommand;