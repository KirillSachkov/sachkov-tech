using SachkovTech.Application.IssueManagement.Commands.AddIssue;

namespace SachkovTech.API.Controllers.Modules.Requests;

public record AddIssueRequest(
    string Title,
    string Description)
{
    public AddIssueCommand ToCommand(Guid moduleId) =>
        new (moduleId, Title, Description);
}