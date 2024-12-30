using MassTransit;

namespace MassTransitLearning.Application.Events
{
    public class MatchRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public DateTime MatchDate { get; init; }
        public required string From { get; init; }
    }

    public class TeamNotification : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public required string Message { get; init; }
    }

    #region Managers
    public class ManagerRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public DateTime MatchDate { get; init; }
    }
    public class ManagerResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    public class ManagerAssistantRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public DateTime MatchDate { get; init; }
    }
    public class ManagerAssistantResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    #endregion

    #region Players
    public class PlayerRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public DateTime MatchDate { get; init; }
    }
    public class PlayerOneResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    public class PlayerTwoResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    public class PlayerThreeResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    public class PlayerFourResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    public class PlayerFiveResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    public class PlayerResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    public class PlayerResponsesCompleted : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }
    #endregion

    public class MatchConfirmation : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public DateTime MatchDate { get; init; }
        public bool Available { get; init; }
    }
}