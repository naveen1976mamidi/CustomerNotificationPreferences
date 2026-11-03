using System.ComponentModel.DataAnnotations;

namespace CustomerNotificationPreferences.Api.Models;

public class UpdateNotificationPreferencesRequest
{
    [Required]
    public bool? EmailEnabled { get; init; }

    [Required]
    public bool? SmsEnabled { get; init; }
}