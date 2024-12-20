using MassTransit;

namespace Oval.MassTransitLearning.API.Events
{
    public class SuccessfulStockCheck : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
}
