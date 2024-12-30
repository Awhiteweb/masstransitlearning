using MassTransit;
using Microsoft.Extensions.Logging;
using MassTransitLearning.Application.Events;

namespace MassTransitLearning.Application.Consumers
{
    public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<TeamNotification>
    {
        public async Task Consume(ConsumeContext<TeamNotification> context)
        {
            // logger.LogInformation("Sending notification {MESSAGE}", context.Message.Message);
            var wait = (int)Math.Round(1000 * Random.Shared.NextSingle());
            await Task.Delay(wait);
            logger.LogInformation("Sent notification {MESSAGE}", context.Message.Message);
        }
    }
    public class PlayerConfirmationConsumer(ILogger<PlayerConfirmationConsumer> logger) : IConsumer<PlayerConfirmation>
    {
        public async Task Consume(ConsumeContext<PlayerConfirmation> context)
        {
            // logger.LogInformation("Received {PLAYER} confirmation", context.Message.Player);
            var message = $"{context.Message.Player} is {(context.Message.Available ? "available" : "unavailable")} to play";
            await context.Publish(new TeamNotification
            {
                CorrelationId = context.Message.CorrelationId,
                Message = message
            });
        }
    }
}