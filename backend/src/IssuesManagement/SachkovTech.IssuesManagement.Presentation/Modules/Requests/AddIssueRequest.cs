using SachkovTech.IssuesManagement.Application.Commands.AddIssue;

namespace SachkovTech.IssuesManagement.Presentation.Modules.Requests;

public record AddIssueRequest(
    string Title,
    string Description)
{
    public AddIssueCommand ToCommand(Guid moduleId) =>
        new (moduleId, Title, Description);
}