namespace SachkovTech.API.Modules;

public record AddIssueRequest(
    string Title,
    string Description,
    IFormFileCollection Files);