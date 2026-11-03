//stub implementation 
using CustomerNotificationPreferences.Api.Models;

namespace CustomerNotificationPreferences.Api.Notifications;

public interface INotificationPublisher
{
    Task PublishAsync(
        Guid customerId,
        NotificationPreferences preferences);
}