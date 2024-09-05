using SachkovTech.Application.Modules.AddIssue;

namespace SachkovTech.API.Controllers.Modules.Requests;

public record AddIssueRequest(
    string Title,
    string Description,
    IFormFileCollection Files)
{
    public AddIssueCommand ToCommand(Guid moduleId, IEnumerable<CreateFileCommand> fileCommands) =>
        new (moduleId, Title, Description, fileCommands);
}