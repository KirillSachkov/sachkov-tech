namespace NotificationService.Entities;

public class Notifications
{
    public Guid Id { get; set; }
    public List<Guid> RoleIds { get; set; } = [];
    public List<Guid> UserIds { get; set; } = [];
    public string Message { get; set; } = string.Empty;
    public bool IsSend { get; set; }
}