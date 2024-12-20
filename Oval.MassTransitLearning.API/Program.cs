using MassTransit;
using Microsoft.OpenApi.Models;
using Oval.MassTransitLearning.API.Events;
using Oval.MassTransitLearning.API.Sagas;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder( args );

builder.Host.UseSerilog( (context, services, configuration) => configuration.MinimumLevel.Debug().WriteTo.Console() );

// Add services to the container.
var configuration = builder.Configuration;
Assembly[] assemblies = [typeof( Program ).Assembly];
const int ConcurrencyLimit = 20;

builder.Services.AddSerilog();
builder.Services.AddSwaggerGen( c => c.SwaggerDoc( "v1", new OpenApiInfo { Title = "MassTransitLearning", Version = "v1" } ) );

builder.Services.Configure<RabbitMqTransportOptions>( builder.Configuration.GetSection( "RabbitMQ" ) );
var rabbitMQConfiguration = configuration.GetSection( "RabbitMQ" ).Get<RabbitMqTransportOptions>();
builder.Services.AddMassTransit( x =>
{
    x.AddSagaStateMachine<OrderStateMachine, OrderState>();
    x.AddConsumers( assemblies );
    x.AddSagas( assemblies );
    x.AddDelayedMessageScheduler();
    x.SetEndpointNameFormatter( new KebabCaseEndpointNameFormatter( includeNamespace: true ) );
    x.UsingRabbitMq( ( context, cfg ) =>
    {
        cfg.Host( rabbitMQConfiguration!.Host, rabbitMQConfiguration.VHost, rabbitMqHostConfigurator =>
        {
            rabbitMqHostConfigurator.Username( rabbitMQConfiguration.User );
            rabbitMqHostConfigurator.Password( rabbitMQConfiguration.Pass );
        } );
        cfg.UseDelayedMessageScheduler();
        cfg.ConfigureEndpoints( context );
        cfg.ReceiveEndpoint( "MassTransitLearning", ec =>
        {
            ec.PrefetchCount = ConcurrencyLimit;
            ec.UseMessageRetry( r => r.Interval( 5, 1000 ) );
            ec.UseInMemoryOutbox( context );
            ec.ConfigureConsumers( context );
            ec.ConfigureSagas( context );
        } );

        cfg.UseInMemoryOutbox( context );

        cfg.AutoStart = true;
    } );
} );

builder.Services.AddOptions<MassTransitHostOptions>().Configure( options => options.WaitUntilStarted = true );


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI( c => c.SwaggerEndpoint( "/swagger/v1/swagger.json", "MassTransitLearning" ) );

app.MapPost( "/create", async ( string[] items, IBus bus ) =>
    await bus.Publish( new OrderCreated()
    {
        CorrelationId = Guid.NewGuid(),
        Items = items
    } ) );

app.Run();

