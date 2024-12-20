using MassTransit;
using Oval.MassTransitLearning.API.Events;

namespace Oval.MassTransitLearning.API.Consumers
{
    public class StockCheckConsumer : IConsumer<StockCheck>
    {
        public async Task Consume( ConsumeContext<StockCheck> context )
        {
            await Task.Delay( 100 );
            var errors = new List<string>();
            foreach( var item in context.Message.Items )
            {
                if( new Random().NextSingle() < 0.5 )
                {
                    errors.Add( $"No stock for the following item {item}" );
                }
            }
            if( errors.Count > 0 )
            {
                throw new Exception( string.Join( ", ", errors ) );
            }

            await context.Publish( new SuccessfulStockCheck()
            {
                CorrelationId = context.Message.CorrelationId
            } );
        }
    }
}
