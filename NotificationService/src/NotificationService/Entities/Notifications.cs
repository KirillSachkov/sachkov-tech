namespace NotificationService.Entities;

public class Notifications
{
    public Guid Id { get; set; }
    public List<Role> Roles { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public string Message { get; set; } = string.Empty;
    public bool IsSend { get; set; }
}