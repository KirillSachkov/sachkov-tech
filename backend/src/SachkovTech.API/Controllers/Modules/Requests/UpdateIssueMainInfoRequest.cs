using SachkovTech.Application.IssueManagement.Commands.UpdateIssueMainInfo;

namespace SachkovTech.API.Controllers.Modules.Requests;

public record UpdateIssueMainInfoRequest(
    string Title,
    string Description,
    int Experience)
{
    public UpdateIssueMainInfoCommand ToCommand(Guid moduleId, Guid issueId) =>
        new(moduleId, issueId, Title, Description, Experience);
}