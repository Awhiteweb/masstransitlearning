using MassTransit;
using Oval.MassTransitLearning.API.Events;

namespace Oval.MassTransitLearning.API.Consumers
{
    public class OrderPlacedConsumer( ILogger<OrderPlacedConsumer> logger ) : IConsumer<OrderPlaced>
    {
        public async Task Consume( ConsumeContext<OrderPlaced> context )
        {
            await Task.Delay( 250 );

            logger.LogInformation( "A notification is being sent about a new order for items {ITEMS}", context.Message.Items );
        }
    }
}
