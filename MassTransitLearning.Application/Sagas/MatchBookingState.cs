using MassTransit;

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
}
