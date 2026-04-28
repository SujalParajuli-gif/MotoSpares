using System.ComponentModel.DataAnnotations;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Domain.Entities;

public class Notification
{
    public int NotificationId { get; set; }

    public NotificationType NotificationType { get; set; }

    [Required]
    [StringLength(500)]
    public string NotificationMessage { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime NotificationDate { get; set; }

    public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
}
