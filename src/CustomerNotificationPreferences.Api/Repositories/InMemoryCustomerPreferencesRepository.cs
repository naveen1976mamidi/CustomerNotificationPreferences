
using CustomerNotificationPreferences.Api.Models;
namespace CustomerNotificationPreferences.Api.Repositories;

public class InMemoryCustomerPreferencesRepository
    : ICustomerPreferencesRepository
{
    private readonly Dictionary<Guid, NotificationPreferences> _preferences;

    public InMemoryCustomerPreferencesRepository()
    {
        _preferences = new Dictionary<Guid, NotificationPreferences>();
    }
}