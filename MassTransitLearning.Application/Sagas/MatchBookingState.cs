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
        public int PlayerResponses { get; set; }
    }
}
