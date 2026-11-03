using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerNotificationPreferences.Tests.Integration;

public class UpdatePreferencesTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UpdatePreferencesTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateExistingCustomer_Returns200()
    {
        var response = await _client.PutAsJsonAsync(
            "/api/customers/11111111-1111-1111-1111-111111111111/notification-preferences",
            new
            {
                emailEnabled = false,
                smsEnabled = true
            });

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateUnknownCustomer_Returns404()
    {
        var response = await _client.PutAsJsonAsync(
            $"/api/customers/{Guid.NewGuid()}/notification-preferences",
            new
            {
                emailEnabled = false,
                smsEnabled = true
            });

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}