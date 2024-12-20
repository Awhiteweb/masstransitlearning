using MassTransit;

namespace Oval.MassTransitLearning.API.Events
{
    public class OrderCreated : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }

        public string[] Items { get; init; } = [];
    }
}
