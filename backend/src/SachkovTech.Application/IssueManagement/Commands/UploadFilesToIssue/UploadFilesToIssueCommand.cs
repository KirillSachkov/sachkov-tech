using SachkovTech.Application.Abstraction;
using SachkovTech.Application.Dtos;

namespace SachkovTech.Application.IssueManagement.Commands.UploadFilesToIssue;

public record UploadFilesToIssueCommand(Guid ModuleId, Guid IssueId, IEnumerable<UploadFileDto> Files) : ICommand;