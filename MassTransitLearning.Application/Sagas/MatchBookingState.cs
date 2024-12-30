using MassTransit;
using MassTransitLearning.Application.Consumers;
using MassTransitLearning.Application.Events;

namespace MassTransitLearning.Application.Sagas
{
    public class MatchBookingState : SagaStateMachineInstance, ISagaVersion
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Version { get; set; }
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }
        public DateTime? MatchDate { get; set; }
        public string? From { get; set; }
        public bool? PlayerOneResponse { get; set; }
        public bool? PlayerTwoResponse { get; set; }
        public bool? PlayerThreeResponse { get; set; }
        public bool? PlayerFourResponse { get; set; }
        public bool? PlayerFiveResponse { get; set; }

        #region Helpers
        public bool HaveAllPlayersResponded => PlayerOneResponse.HasValue
            && PlayerTwoResponse.HasValue
            && PlayerThreeResponse.HasValue
            && PlayerFourResponse.HasValue
            && PlayerFiveResponse.HasValue;
        public bool IsTeamAvailable => PlayerOneResponse.HasValue && PlayerOneResponse.Value
            && PlayerTwoResponse.HasValue && PlayerTwoResponse.Value
            && PlayerThreeResponse.HasValue && PlayerThreeResponse.Value
            && PlayerFourResponse.HasValue && PlayerFourResponse.Value
            && PlayerFiveResponse.HasValue && PlayerFiveResponse.Value;
        #endregion
    }

    public class MatchBookingStateSagaDefinition : SagaDefinition<MatchBookingState>
    {
        const int ConcurrencyLimit = 5;
        public MatchBookingStateSagaDefinition()
        {
            Endpoint(e => e.ConcurrentMessageLimit = ConcurrencyLimit);
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<MatchBookingState> sagaConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseInMemoryOutbox(context);
            endpointConfigurator.UseMessageRetry(r =>
            {
                r.Interval(5, 1000);
                r.Ignore(typeof(PlayerUnavailableException));
            });
            var partition = endpointConfigurator.CreatePartitioner(ConcurrencyLimit);
            
            sagaConfigurator.Message<PlayerConfirmation>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
            sagaConfigurator.Message<PlayerResponse>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
            sagaConfigurator.Message<PlayerOneResponse>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
            sagaConfigurator.Message<PlayerTwoResponse>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
            sagaConfigurator.Message<PlayerThreeResponse>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
            sagaConfigurator.Message<PlayerFourResponse>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
            sagaConfigurator.Message<PlayerFiveResponse>(x => x.UsePartitioner(partition, m => m.Message.CorrelationId));
        }
    }
}
