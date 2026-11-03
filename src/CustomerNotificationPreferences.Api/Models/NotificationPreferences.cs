namespace CustomerNotificationPreferences.Api.Models;

public class NotificationPreferences
{
    public Guid CustomerId { get; set; }

    public bool EmailEnabled { get; set; }

    public bool SmsEnabled { get; set; }
}