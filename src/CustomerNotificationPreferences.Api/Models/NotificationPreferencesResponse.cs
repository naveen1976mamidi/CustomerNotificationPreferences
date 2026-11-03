namespace CustomerNotificationPreferences.Api.Models;

public class NotificationPreferencesResponse
{
    public Guid CustomerId { get; init; }

    public bool EmailEnabled { get; init; }

    public bool SmsEnabled { get; init; }
}