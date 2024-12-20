using MassTransit;
using Oval.MassTransitLearning.API.Events;

namespace Oval.MassTransitLearning.API.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public async Task Consume( ConsumeContext<OrderCreated> context )
        {
            await Task.Delay( 1000 );

            if( context.Message.Items.Length == 0 )
            {
                throw new Exception( "There are not items in the order" );
            }

            if( context.Message.Items.Contains( "heros" ) )
            {
                throw new Exception( "There are no heros left to order" );
            }

            await context.Publish( new OrderPlaced()
            {
                CorrelationId = context.Message.CorrelationId,
                Items = context.Message.Items,
            } );
        }
    }
}
