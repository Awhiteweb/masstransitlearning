using MassTransit;

namespace Oval.MassTransitLearning.API.Sagas
{
    public class OrderState : SagaStateMachineInstance, ISagaVersion
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Version { get; set; }
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }
        public string[] Items { get; set; } = [];
    }
}
