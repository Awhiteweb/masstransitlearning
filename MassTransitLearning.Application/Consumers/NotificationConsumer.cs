using MassTransit;
using Microsoft.Extensions.Logging;
using MassTransitLearning.Application.Events;

namespace MassTransitLearning.Application.Consumers
{
    public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<TeamNotification>
    {
        public async Task Consume(ConsumeContext<TeamNotification> context)
        {
            logger.LogInformation("Sending notification {MESSAGE}", context.Message.Message);
            var wait = (int)Math.Round(1000 * Random.Shared.NextSingle());
            await Task.Delay(wait);
            logger.LogInformation("Sent notification {MESSAGE}", context.Message.Message);
        }
    }
}