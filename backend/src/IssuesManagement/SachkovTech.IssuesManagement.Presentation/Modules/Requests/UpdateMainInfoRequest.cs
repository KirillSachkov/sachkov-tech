using SachkovTech.IssuesManagement.Application.Commands.UpdateMainInfo;

namespace SachkovTech.IssuesManagement.Presentation.Modules.Requests;

public record UpdateMainInfoRequest(
    string Title,
    string Description)
{
    public UpdateMainInfoCommand ToCommand(Guid moduleId) =>
        new(moduleId, Title, Description);
}