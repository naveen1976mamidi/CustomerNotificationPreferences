using system;
using System.Threading.Tasks;
using CustomerNotificationPreferences.Api.Models;
using CustomerNotificationPreferences.Api.Results;
using CustomerNotificationPreferences.Api.Requests;

// Interface : Business logic interface for managing notification preferences
public interface INotificationPreferencesService
{
    Task<NotificationPreferences?> GetAsync(Guid customerId);

    Task<UpdateResult> UpdateAsync(
        Guid customerId,
        UpdateNotificationPreferencesRequest request);
}