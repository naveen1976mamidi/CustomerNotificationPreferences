// Result
// Service result object. Needed to tell controller:
//success
//customer missing
//notification failure
public record UpdateResult(
    bool Success,
    bool NotificationFailed,
    NotificationPreferences? Preferences);