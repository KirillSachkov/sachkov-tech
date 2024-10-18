namespace NotificationService.Entities;

public class Notification
{
    public Guid Id { get; init; }

    public List<Guid> RoleIds { get; init; } = [];

    public List<Guid> UserIds { get; init; } = [];

    public string Message { get; init; } = string.Empty;

    public bool IsSend { get; init; }
}