using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerNotificationPreferences.Tests.Integration;

public class GetPreferencesTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetPreferencesTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ExistingCustomer_Returns200()
    {
        var response = await _client.GetAsync(
            "/api/customers/11111111-1111-1111-1111-111111111111/notification-preferences");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task UnknownCustomer_Returns404()
    {
        var response = await _client.GetAsync(
            $"/api/customers/{Guid.NewGuid()}/notification-preferences");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}