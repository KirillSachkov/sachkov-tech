namespace NotificationService.Entities;

public class NotificationSettings
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public bool Email { get; init; } = true;

    public bool Telegram { get; init; }

    public bool Web { get; init; } = true;
}