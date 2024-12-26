using MassTransit;
using Microsoft.Extensions.Logging;
using MassTransitLearning.Application.Events;

namespace MassTransitLearning.Application.Sagas
{
    public class MatchBookingStateMachine : MassTransitStateMachine<MatchBookingState>
    {
        public State? MatchRequested { get; private set; }
        public State? ManagersConfirmed { get; private set; }
        public State? TeamConfirmed { get; private set; }
        public State? ManagersUnavailable { get; private set; }
        public State? TeamUnavailable { get; private set; }

        public Event? PlayerResponsesCompleted { get; private set; }
        public Event<MatchRequest>? MatchRequest { get; private set; }
        public Event<Fault<ManagerRequest>>? ManagerRequestFault { get; private set; }
        public Event<ManagerResponse>? ManagerResponse { get; private set; }
        public Event<ManagerAssistantResponse>? ManagerAssistantResponse { get; private set; }
        public Event<Fault<ManagerAssistantRequest>>? ManagerAssistantRequestFault { get; private set; }
        public Event<PlayerOneResponse>? PlayerOneResponse { get; private set; }
        public Event<PlayerTwoResponse>? PlayerTwoResponse { get; private set; }
        public Event<PlayerThreeResponse>? PlayerThreeResponse { get; private set; }
        public Event<PlayerFourResponse>? PlayerFourResponse { get; private set; }
        public Event<PlayerFiveResponse>? PlayerFiveResponse { get; private set; }
        public Event<Fault<PlayerRequest>>? PlayerRequestFault { get; private set; }

        public MatchBookingStateMachine(ILogger<MatchBookingStateMachine> logger)
        {
            Event(() => MatchRequest, e => 
            {
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => new MatchBookingState()
                {
                    CorrelationId = context.Message.CorrelationId,
                    MatchDate = context.Message.MatchDate,
                    From = context.Message.From,
                });
            });

            InstanceState(x => x.CurrentState);

            Initially(
                When(MatchRequest)
                .Then(context => logger.LogInformation("A match booking has been registered with the state machine"))
                .TransitionTo(MatchRequested)
                .Publish(context => context.Publish<ManagerRequest>(new
                {
                    context.Saga.CorrelationId,
                    context.Saga.MatchDate,
                    context.Saga.From
                })));

            During(MatchRequested,
                When(ManagerResponse)
                    .Publish(context => context.Publish<ManagerAssistantRequest>(new
                    {
                        context.Saga.CorrelationId,
                        context.Saga.MatchDate,
                        context.Saga.From
                    })),
                When(ManagerAssistantResponse)
                    .TransitionTo(ManagersConfirmed)
                    .Publish(context => context.Publish<PlayerRequest>(new
                    {
                        context.Saga.CorrelationId,
                        context.Saga.MatchDate,
                    })));

            During(ManagersConfirmed,
                When(PlayerResponsesCompleted)
                    .Then(_ => logger.LogInformation("All players have been confirmed"))
                    .Publish(context => context.Publish<TeamNotification>(new
                    {
                        context.Saga.CorrelationId,
                        Message = "All players have been confirmed"
                    }))
                    .Finalize());

            DuringAny(
                When(ManagerRequestFault)
                    .Then(context => logger.LogError("{MESSAGE}", context.Message.Exceptions[0].Message))
                    .TransitionTo(ManagersUnavailable),
                When(ManagerAssistantRequestFault)
                    .Then(context => logger.LogError("{MESSAGE}", context.Message.Exceptions[0].Message))
                    .TransitionTo(ManagersUnavailable),
                When(PlayerRequestFault)
                    .Then(context => logger.LogError("{MESSAGE}", context.Message.Exceptions[0].Message))
                    .TransitionTo(TeamUnavailable));
            
            CompositeEvent(
                () => PlayerResponsesCompleted,
                s => s.PlayerResponses,
                PlayerOneResponse,
                PlayerTwoResponse,
                PlayerThreeResponse,
                PlayerFourResponse,
                PlayerFiveResponse
            );

        }
    }
}
