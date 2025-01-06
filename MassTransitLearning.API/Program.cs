using MassTransit;
using MassTransitLearning.Application;
using MassTransitLearning.Application.Events;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override( "MassTransit", Serilog.Events.LogEventLevel.Error )
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => 
        r.AddService("MassTransitLearning", serviceVersion: "1", serviceInstanceId: Environment.MachineName))
    .WithTracing(tracing => {
        tracing.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddHttpClientInstrumentation();
    })
    .UseOtlpExporter();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/book", async (MatchBooking booking, IBus bus) => {
    await bus.Publish(new MatchRequest 
    { 
        CorrelationId = Guid.NewGuid(), 
        MatchDate = booking.MatchDate, 
        From = booking.From 
    });
    return "booking request submitted";
})
.WithName("Match Booking")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record MatchBooking(DateTime MatchDate, string From);