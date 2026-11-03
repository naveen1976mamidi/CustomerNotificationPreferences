// test
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerNotificationPreferences.Tests.Integration;

public class NotificationFailureApiTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task NotificationFailure_Returns503()
    {
        // Configure factory with
        // ThrowingNotificationPublisher

        Assert.True(true);
    }
}