using MassTransit;

namespace MassTransitLearning.Application.Events
{
    public record MatchRequest(Guid CorrelationId, DateTime MatchDate, string From) : CorrelatedBy<Guid>;

    public record TeamNotification(Guid CorrelationId, string Message) : CorrelatedBy<Guid>;

    public record ManagerRequest(Guid CorrelationId, DateTime MatchDate) : CorrelatedBy<Guid>;
    public record ManagerResponse(Guid CorrelationId) : CorrelatedBy<Guid>;
    public record ManagerAssistantRequest(Guid CorrelationId, DateTime MatchDate) : CorrelatedBy<Guid>;
    public record ManagerAssistantResponse(Guid CorrelationId) : CorrelatedBy<Guid>;

    public record PlayerRequest(Guid CorrelationId, DateTime MatchDate) : CorrelatedBy<Guid>;
    public record PlayerOneResponse(Guid CorrelationId) : CorrelatedBy<Guid>;
    public record PlayerTwoResponse(Guid CorrelationId) : CorrelatedBy<Guid>;
    public record PlayerThreeResponse(Guid CorrelationId) : CorrelatedBy<Guid>;
    public record PlayerFourResponse(Guid CorrelationId) : CorrelatedBy<Guid>;
    public record PlayerFiveResponse(Guid CorrelationId) : CorrelatedBy<Guid>;

    public record MatchConfirmation(Guid CorrelationId, DateTime MatchDate, bool Available) : CorrelatedBy<Guid>;
}