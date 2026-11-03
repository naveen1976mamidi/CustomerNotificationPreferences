Design note
The solution uses a simple layered design with Controllers, Services, Repositories and Notifications separated by responsibility.

The controller layer is responsible only for HTTP concerns, including routing and translating service results into the appropriate HTTP responses. Business rules are implemented in NotificationPreferencesService, which contains the logic for retrieving preferences, updating preferences, determining whether a change has occurred and invoking the notification dependency when required.

Customer data is accessed through ICustomerPreferencesRepository. An in-memory implementation was chosen to satisfy the exercise requirements and provide a simple persistence mechanism without introducing database dependencies. The repository is registered as a singleton so that updates persist for the lifetime of the application.

The external notification system is represented by INotificationPublisher. A fake implementation is provided, allowing the application to satisfy the requirement without making any network calls. This abstraction also simplifies testing by allowing mocks and test doubles to be substituted.

Validation uses ASP.NET Core model validation with nullable booleans and Required attributes to ensure omitted properties are rejected instead of silently defaulting to false.

Testing follows a test-first approach where practical. Unit tests focus on business behaviour within the service layer, including notification interactions. Integration tests verify routing, validation, status codes and persistence behaviour through the API endpoints.

Known limitations include in-memory storage, no authentication, minimal logging and a simplified notification failure policy. In a production implementation I would consider a persistent database, structured logging, telemetry, retry strategies and an Outbox pattern to provide more reliable integration with external notification systems.