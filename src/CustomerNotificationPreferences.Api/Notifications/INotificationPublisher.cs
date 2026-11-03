using System;
using System.Threading.Tasks;
using CustomerNotificationPreferences.Api.Models;
//External dependency represented as interface.

namespace CustomerNotificationPreferences.Api.Notifications;
public interface INotificationPublisher
{
    Task PublishAsync(
        Guid customerId,
        NotificationPreferences preferences);
}