namespace MotoSpares.Domain.Entities;

public class UserNotification
{
    public Guid UserId { get; set; }
    public int NotificationId { get; set; }

    public ApplicationUser? User { get; set; }
    public Notification? Notification { get; set; }
}
