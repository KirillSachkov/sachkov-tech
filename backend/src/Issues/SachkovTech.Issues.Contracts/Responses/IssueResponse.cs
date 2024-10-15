namespace SachkovTech.Issues.Contracts.Responses
{
    public record IssueResponse(
        Guid Id,
        Guid ModuleId,
        string Title,
        string Description,
        int Position,
        Guid? LessonId,
        FileResponse[] Files);

    public record FileResponse(
        Guid Id,
        string Link
        );
}
