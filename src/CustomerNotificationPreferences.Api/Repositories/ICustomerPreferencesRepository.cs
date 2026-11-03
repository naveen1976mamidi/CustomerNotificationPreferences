using CustomerNotificationPreferences.Api.Models;
namespace CustomerNotificationPreferences.Api.Repositories;
// Repo
// Abstract storage.

public interface ICustomerPreferencesRepository
{
    NotificationPreferences? Get(Guid customerId);

    void Update(NotificationPreferences preferences);
}