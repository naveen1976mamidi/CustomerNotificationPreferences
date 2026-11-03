// tests
using Moq;
using CustomerNotificationPreferences.Api.Models;
using CustomerNotificationPreferences.Api.Notifications;
using CustomerNotificationPreferences.Api.Repositories;
using CustomerNotificationPreferences.Api.Services;

namespace CustomerNotificationPreferences.Tests.Services;

public class NotificationPreferencesServiceTests
{
    private readonly InMemoryCustomerPreferencesRepository _repository;
    private readonly Mock<INotificationPublisher> _publisher;
    private readonly NotificationPreferencesService _service;

    public NotificationPreferencesServiceTests()
    {
        _repository = new InMemoryCustomerPreferencesRepository();

        _publisher = new Mock<INotificationPublisher>();

        _service = new NotificationPreferencesService(
            _repository,
            _publisher.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsCustomer_WhenCustomerExists()
    {
        var customerId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        var result = await _service.GetAsync(customerId);

        Assert.NotNull(result);
        Assert.Equal(customerId, result.CustomerId);
    }

    [Fact]
    public async Task GetAsync_ReturnsNull_WhenCustomerDoesNotExist()
    {
        var result =
            await _service.GetAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFailure_WhenCustomerDoesNotExist()
    {
        var result = await _service.UpdateAsync(
            Guid.NewGuid(),
            new UpdateNotificationPreferencesRequest
            {
                EmailEnabled = true,
                SmsEnabled = true
            });

        Assert.False(result.Success);
        Assert.Null(result.Preferences);
    }

    [Fact]
    public async Task UpdateAsync_ChangedValues_TriggersNotification()
    {
        var customerId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        await _service.UpdateAsync(
            customerId,
            new UpdateNotificationPreferencesRequest
            {
                EmailEnabled = false,
                SmsEnabled = true
            });

        _publisher.Verify(
            p => p.PublishAsync(
                customerId,
                It.IsAny<NotificationPreferences>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UnchangedValues_DoesNotTriggerNotification()
    {
        var customerId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        await _service.UpdateAsync(
            customerId,
            new UpdateNotificationPreferencesRequest
            {
                EmailEnabled = true,
                SmsEnabled = false
            });

        _publisher.Verify(
            p => p.PublishAsync(
                It.IsAny<Guid>(),
                It.IsAny<NotificationPreferences>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_NotificationFailure_ReturnsFlag()
    {
        var customerId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        _publisher
            .Setup(p => p.PublishAsync(
                It.IsAny<Guid>(),
                It.IsAny<NotificationPreferences>()))
            .ThrowsAsync(new Exception());

        var result = await _service.UpdateAsync(
            customerId,
            new UpdateNotificationPreferencesRequest
            {
                EmailEnabled = false,
                SmsEnabled = true
            });

        Assert.True(result.Success);
        Assert.True(result.NotificationFailed);
    }
}