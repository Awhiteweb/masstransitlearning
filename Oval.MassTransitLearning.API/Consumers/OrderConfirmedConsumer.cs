using MassTransit;
using Oval.MassTransitLearning.API.Events;

namespace Oval.MassTransitLearning.API.Consumers
{
    public class OrderConfirmedConsumer( ILogger<OrderConfirmedConsumer> logger ) : IConsumer<OrderConfirmed>
    {
        public Task Consume( ConsumeContext<OrderConfirmed> context )
        {
            logger.LogInformation( "An order has been confirmed" );
            return Task.CompletedTask;
        }
    }
}
