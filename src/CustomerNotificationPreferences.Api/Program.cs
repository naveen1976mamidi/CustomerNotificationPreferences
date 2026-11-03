// Program
using CustomerNotificationPreferences.Api.Notifications;
using CustomerNotificationPreferences.Api.Repositories;
using CustomerNotificationPreferences.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<
    ICustomerPreferencesRepository,
    InMemoryCustomerPreferencesRepository>();

builder.Services.AddScoped<
    INotificationPreferencesService,
    NotificationPreferencesService>();

builder.Services.AddScoped<
    INotificationPublisher,
    FakeNotificationPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

