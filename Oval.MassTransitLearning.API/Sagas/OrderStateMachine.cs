using MassTransit;
using Oval.MassTransitLearning.API.Events;

namespace Oval.MassTransitLearning.API.Sagas
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State? OrderCreated { get; private set; }
        public State? OrderUpdated { get; private set; }
        public State? OrderCompleted { get; private set; }
        public State? OrderFailed { get; private set; }

        public Event<OrderCreated>? OrderCreatedEvent { get; private set; }
        public Event<OrderPlaced>? OrderPlacedEvent { get; private set; }
        public Event<SuccessfulStockCheck>? SuccessfulStockCheckEvent { get; private set; }

        public Event<Fault<OrderCreated>>? OrderCreatedFault {  get; private set; }
        public Event<Fault<StockCheck>>? StockCheckFault {  get; private set; }

        public OrderStateMachine( ILogger<OrderStateMachine> logger )
        {
            InstanceState( x => x.CurrentState );

            Initially(
                When( OrderCreatedEvent )
                .Then( context =>
                {
                    context.Saga.Items = context.Message.Items;
                    logger.LogInformation( "A new order has been registered with the state machine" );
                } )
                .TransitionTo( OrderCreated ) );

            During( OrderCreated,
                When( OrderPlacedEvent )
                .Publish( context => new StockCheck() { CorrelationId = context.Message.CorrelationId, Items = context.Message.Items } ),

                When( SuccessfulStockCheckEvent )
                .Publish( context => new OrderConfirmed() { CorrelationId = context.Message.CorrelationId, Items = context.Saga.Items } )
                .TransitionTo( OrderCompleted )
                .Finalize() );

            DuringAny(
                When( OrderCreatedFault )
                .Then( context => logger.LogError( "Order created fault {MESSAGE}", context.Message.Exceptions[0].Message ) ),
                When( StockCheckFault )
                .Then( context => logger.LogError( "Stock check fault {MESSAGE}", context.Message.Exceptions[0].Message ) ) );
        }
    }
}
