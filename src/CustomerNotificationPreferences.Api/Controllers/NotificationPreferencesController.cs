// Controller
using CustomerNotificationPreferences.Api.Models;
using CustomerNotificationPreferences.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerNotificationPreferences.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId:guid}/notification-preferences")]
public class NotificationPreferencesController : ControllerBase
{
    private readonly INotificationPreferencesService _service;

    public NotificationPreferencesController(
        INotificationPreferencesService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(NotificationPreferencesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid customerId)
    {
        var preferences = await _service.GetAsync(customerId);

        if (preferences is null)
        {
            return NotFound();
        }

        return Ok(new NotificationPreferencesResponse
        {
            CustomerId = preferences.CustomerId,
            EmailEnabled = preferences.EmailEnabled,
            SmsEnabled = preferences.SmsEnabled
        });
    }

    [HttpPut]
    [ProducesResponseType(typeof(NotificationPreferencesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Put(
        Guid customerId,
        [FromBody] UpdateNotificationPreferencesRequest request)
    {
        var result = await _service.UpdateAsync(
            customerId,
            request);

        if (result.Preferences is null)
        {
            return NotFound();
        }

        if (result.NotificationFailed)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new ErrorResponse
                {
                    Message =
                        "Preferences were saved but notification failed."
                });
        }

        return Ok(new NotificationPreferencesResponse
        {
            CustomerId = result.Preferences.CustomerId,
            EmailEnabled = result.Preferences.EmailEnabled,
            SmsEnabled = result.Preferences.SmsEnabled
        });
    }
}