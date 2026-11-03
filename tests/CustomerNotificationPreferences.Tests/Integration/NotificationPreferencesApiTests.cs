using Moq;
using CustomerNotificationPreferences.Api.Models;
using CustomerNotificationPreferences.Api.Notifications;
using CustomerNotificationPreferences.Api.Repositories;
using CustomerNotificationPreferences.Api.Services;

namespace CustomerNotificationPreferences.Tests.Services;

public class NotificationPreferencesServiceTests
{
    [Fact]
    public async Task GetAsync_ReturnsCustomer_WhenCustomerExists()
    {
        var repository = new InMemoryCustomerPreferencesRepository();

        var publisher = new Mock<INotificationPublisher>();

        var service = new NotificationPreferencesService(
            repository,
            publisher.Object);

        var customerId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        var result = await service.GetAsync(customerId);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ReturnsNull_WhenCustomerMissing()
    {
        var repository = new InMemoryCustomerPreferencesRepository();

        var publisher = new Mock<INotificationPublisher>();

        var service = new NotificationPreferencesService(
            repository,
            publisher.Object);

        var result =
            await service.GetAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ChangedValues_TriggersNotification()
    {
        var repository =
            new InMemoryCustomerPreferencesRepository();

        var publisher =
            new Mock<INotificationPublisher>();

        var service =
            new NotificationPreferencesService(
                repository,
                publisher.Object);

        var customerId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        await service.UpdateAsync(
            customerId,
            new UpdateNotificationPreferencesRequest
            {
                EmailEnabled = false,
                SmsEnabled = true
            });

        publisher.Verify(
            p => p.PublishAsync(
                customerId,
                It.IsAny<NotificationPreferences>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UnchangedValues_DoesNotNotify()
    {
        var repository =
            new InMemoryCustomerPreferencesRepository();

        var publisher =
            new Mock<INotificationPublisher>();

        var service =
            new NotificationPreferencesService(
                repository,
                publisher.Object);

        var customerId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        await service.UpdateAsync(
            customerId,
            new UpdateNotificationPreferencesRequest
            {
                EmailEnabled = true,
                SmsEnabled = false
            });

        publisher.Verify(
            p => p.PublishAsync(
                It.IsAny<Guid>(),
                It.IsAny<NotificationPreferences>()),
            Times.Never);
    }
}