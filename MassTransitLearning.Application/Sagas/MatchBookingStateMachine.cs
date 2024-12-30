using MassTransit;
using MassTransitLearning.Application.Consumers;
using MassTransitLearning.Application.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MassTransitLearning.Application.Sagas
{
    public class MatchBookingStateMachine : MassTransitStateMachine<MatchBookingState>
    {
        public State? MatchRequested { get; private set; }
        public State? ConfirmingTeam { get; private set; }
        public State? TeamConfirmed { get; private set; }
        public State? ManagersUnavailable { get; private set; }
        public State? TeamUnavailable { get; private set; }

        public Event<PlayerResponsesCompleted>? PlayerResponsesCompleted { get; private set; }
        public Event<MatchRequest>? MatchRequest { get; private set; }
        public Event<PlayerOneResponse>? PlayerOneResponse { get; private set; }
        public Event<PlayerTwoResponse>? PlayerTwoResponse { get; private set; }
        public Event<PlayerThreeResponse>? PlayerThreeResponse { get; private set; }
        public Event<PlayerFourResponse>? PlayerFourResponse { get; private set; }
        public Event<PlayerFiveResponse>? PlayerFiveResponse { get; private set; }
        public Event<PlayerResponse>? PlayerResponse { get; private set; }
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
                    .Then(context => logger.LogInformation("A match booking has been registered with the state machine, {CTX}", context.Saga.CorrelationId))
                    .TransitionTo(MatchRequested)
                    .PublishAsync(context => context.Init<PlayerRequest>(new
                    {
                        context.Saga.CorrelationId,
                        MatchDate = context.Saga.MatchDate!.Value
                    }))
                );

            During(MatchRequested,
                When(PlayerOneResponse).ConfirmPlayer("Player One"),
                When(PlayerTwoResponse).ConfirmPlayer("Player Two"),
                When(PlayerThreeResponse).ConfirmPlayer("Player Three"),
                When(PlayerFourResponse).ConfirmPlayer("Player Four"),
                When(PlayerFiveResponse).ConfirmPlayer("Player Five"),
                When(PlayerResponse)
                    .If(
                        context => context.Saga.HaveAllPlayersResponded,
                        context => context.IfElse(
                            ctx => ctx.Saga.IsTeamAvailable,
                            ctx => ctx.TransitionTo(TeamConfirmed)
                                      .PublishAsync(ctx => ctx.Init<PlayerResponsesCompleted>(new {ctx.CorrelationId})),
                            ctx => ctx.TransitionTo(TeamUnavailable)
                                      .PublishAsync(ctx => ctx.Init<PlayerResponsesCompleted>(new {ctx.CorrelationId}))))
                );

            DuringAny(
                When(PlayerResponsesCompleted)
                    .Then(context => logger.LogInformation("All players have confirmed, {CTX}", context.Saga.CorrelationId))
                    .PublishAsync(context => context.Init<TeamNotification>(new
                    {
                        context.Saga.CorrelationId,
                        Message = $"All players have confirmed and the team is {(context.Saga.IsTeamAvailable ? "available" : "unavailable")} to play"
                    }))
                    .Finalize());

            DuringAny(
                When(PlayerRequestFault)
                    .Then(context => context.Saga.SetPlayerStatus(context.Message.GetPlayerUnavailableException().Player, false))
                    .PublishAsync(context => context.Init<TeamNotification>(new
                    {
                        context.Saga.CorrelationId,
                        Message = "A player is unavailable"
                    }))
                    .PublishAsync(context => context.Init<PlayerResponse>(new {context.Saga.CorrelationId})));
        }
    }

    public static class SagaExtensions
    {
        public static EventActivityBinder<MatchBookingState, TEvent> ConfirmPlayer<TEvent>(this EventActivityBinder<MatchBookingState, TEvent> binder, string player) where TEvent : class =>
            binder.Then(context => context.Saga.SetPlayerStatus(player, true))
                .PublishAsync(context => context.Init<PlayerResponse>(new {context.Saga.CorrelationId}));

        public static MatchBookingState SetPlayerStatus(this MatchBookingState state, string player, bool available)
        {
            switch(player)
            {
                case "Player One":
                    state.PlayerOneResponse = available;
                    break;
                case "Player Two":
                    state.PlayerTwoResponse = available;
                    break;
                case "Player Three":
                    state.PlayerThreeResponse = available;
                    break;
                case "Player Four":
                    state.PlayerFourResponse = available;
                    break;
                case "Player Five":
                    state.PlayerFiveResponse = available;
                    break;
                default:
                    break;
            }
            return state;
        }

        public static PlayerUnavailable GetPlayerUnavailableException(this Fault<PlayerRequest> fault)
        {
            var exName = typeof(PlayerUnavailableException).FullName;
            var ex = fault.Exceptions
                .Where(x => x.ExceptionType == exName || x.InnerException?.ExceptionType == exName)
                .Select(x => x.ExceptionType == exName ? x : x.InnerException!)
                .First();
            return JsonSerializer.Deserialize<PlayerUnavailable>(ex.Message)!;
        }
    }
}
