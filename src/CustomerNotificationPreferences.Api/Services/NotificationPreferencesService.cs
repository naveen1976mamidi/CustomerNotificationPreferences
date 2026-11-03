// Service
using CustomerNotificationPreferences.Api.Models;
using CustomerNotificationPreferences.Api.Notifications;
using CustomerNotificationPreferences.Api.Repositories;
using CustomerNotificationPreferences.Api.Results;

namespace CustomerNotificationPreferences.Api.Services;

public class NotificationPreferencesService
    : INotificationPreferencesService
{
    private readonly ICustomerPreferencesRepository _repository;
    private readonly INotificationPublisher _notificationPublisher;

    public NotificationPreferencesService(
        ICustomerPreferencesRepository repository,
        INotificationPublisher notificationPublisher)
    {
        _repository = repository;
        _notificationPublisher = notificationPublisher;
    }

    public Task<NotificationPreferences?> GetAsync(Guid customerId)
    {
        return Task.FromResult(
            _repository.Get(customerId));
    }

    public async Task<UpdateResult> UpdateAsync(
        Guid customerId,
        UpdateNotificationPreferencesRequest request)
    {
        var existingPreferences =
            _repository.Get(customerId);

        if (existingPreferences is null)
        {
            return new UpdateResult(
                Success: false,
                NotificationFailed: false,
                Preferences: null);
        }

        var hasChanged =
            existingPreferences.EmailEnabled != request.EmailEnabled!.Value ||
            existingPreferences.SmsEnabled != request.SmsEnabled!.Value;

        existingPreferences.EmailEnabled =
            request.EmailEnabled.Value;

        existingPreferences.SmsEnabled =
            request.SmsEnabled.Value;

        _repository.Update(existingPreferences);

        if (!hasChanged)
        {
            return new UpdateResult(
                Success: true,
                NotificationFailed: false,
                Preferences: existingPreferences);
        }

        try
        {
            await _notificationPublisher.PublishAsync(
                customerId,
                existingPreferences);

            return new UpdateResult(
                Success: true,
                NotificationFailed: false,
                Preferences: existingPreferences);
        }
        catch
        {
            return new UpdateResult(
                Success: true,
                NotificationFailed: true,
                Preferences: existingPreferences);
        }
    }
}