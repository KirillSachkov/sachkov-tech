using SachkovTech.Application.Modules.AddIssue;
using SachkovTech.Application.Modules.UploadFilesToIssue;

namespace SachkovTech.API.Controllers.Modules.Requests;

public record AddIssueRequest(
    string Title,
    string Description)
{
    public AddIssueCommand ToCommand(Guid moduleId) =>
        new (moduleId, Title, Description);
}