using SachkovTech.Issues.Application.Commands.AddIssue;

namespace SachkovTech.Issues.Presentation.Modules.Requests;

public record AddIssueRequest(
    string Title,
    string Description,
    int Experience)
{
    public AddIssueCommand ToCommand(Guid moduleId) =>
        new (moduleId, Title, Description, Experience);
}