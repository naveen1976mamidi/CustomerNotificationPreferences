# Customer Notification Preferences API
# Customer Notification Preferences API

A small ASP.NET Core Web API that allows customers to retrieve and update their notification preferences.

The application stores customer preferences in-memory and notifies a simulated external notification dependency when preference changes occur.

---

# Technology

- .NET 8 (or .NET 10 if installed)
- ASP.NET Core Web API
- xUnit
- Moq
- Microsoft.AspNetCore.Mvc.Testing

---

# Prerequisites

Install:

- .NET SDK 8.0 or later

Verify installation:

```bash
dotnet --version
```

---

# Build

```bash
dotnet build
```

---

# Run

```bash
dotnet run --project src/CustomerNotificationPreferences.Api
```

By default Swagger UI will be available when running in Development mode.

---

# Run Tests

```bash
dotnet test
```

---

# Seed Data

The in-memory repository is seeded with the following customers:

| Customer | Customer ID |
|-----------|-----------|
| Customer A | 11111111-1111-1111-1111-111111111111 |
| Customer B | 22222222-2222-2222-2222-222222222222 |

---

# API Endpoints

## Get Notification Preferences

```http
GET /api/customers/{customerId}/notification-preferences
```

Example:

```http
GET /api/customers/11111111-1111-1111-1111-111111111111/notification-preferences
```

Example Response:

```json
{
  "customerId": "11111111-1111-1111-1111-111111111111",
  "emailEnabled": true,
  "smsEnabled": false
}
```

---

## Update Notification Preferences

```http
PUT /api/customers/{customerId}/notification-preferences
```

Example Request:

```json
{
  "emailEnabled": false,
  "smsEnabled": true
}
```

Example Response:

```json
{
  "customerId": "11111111-1111-1111-1111-111111111111",
  "emailEnabled": false,
  "smsEnabled": true
}
```

---

# Error Responses

## Unknown Customer

```http
404 Not Found
```

## Invalid Request

Examples:

- Missing property
- Invalid JSON
- Invalid GUID

Response:

```http
400 Bad Request
```

## Notification Failure

Response:

```http
503 Service Unavailable
```

Example:

```json
{
  "message": "Preferences were saved but notification failed."
}
```

---

# Notification Failure Behaviour

As required by the exercise, preference updates are persisted before the notification dependency is called.

If notification succeeds:

1. Preferences are updated.
2. Notification is sent.
3. HTTP 200 is returned.

If notification fails:

1. Preferences are updated.
2. Notification fails.
3. HTTP 503 is returned.
4. Updated preferences remain stored.

This behaviour intentionally follows the assessment requirements. A production implementation would typically use an outbox pattern, message queue, retry mechanism, or background processing to ensure eventual notification delivery while maintaining consistency between persisted data and downstream systems. This approach would reduce the risk of lost notifications caused by transient failures in external dependencies.

AI Assistance Declaration: Microsoft Copilot was used as a productivity aid for code suggestions, documentation refinement, and technical guidance. The final solution, architecture, and implementation decisions were independently reviewed and verified by me (Naveen).