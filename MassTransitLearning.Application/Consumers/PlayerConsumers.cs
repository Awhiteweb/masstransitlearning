using MassTransit;
using Microsoft.Extensions.Logging;
using MassTransitLearning.Application.Events;

namespace MassTransitLearning.Application.Consumers
{
    public class PlayerOneConsumer(ILogger<PlayerOneConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player one is available");
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException("Player one");
            }
            logger.LogInformation("Player one is available");
            await context.Publish<PlayerOneResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerTwoConsumer(ILogger<PlayerTwoConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player two is available");
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException("Player two");
            }
            logger.LogInformation("Player two is available");
            await context.Publish<PlayerTwoResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerThreeConsumer(ILogger<PlayerThreeConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player three is available");
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException("Player three");
            }
            logger.LogInformation("Player three is available");
            await context.Publish<PlayerThreeResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerFourConsumer(ILogger<PlayerFourConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player four is available");
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException("Player four");
            }
            logger.LogInformation("Player four is available");
            await context.Publish<PlayerFourResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerFiveConsumer(ILogger<PlayerFiveConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player five is available");
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException("Player five");
            }
            logger.LogInformation("Player five is available");
            await context.Publish<PlayerFiveResponse>(new {context.Message.CorrelationId});
        }
    }

    public class PlayerUnavailableException(string player) : Exception( $"{player} is unavailable") { }

    public static class PlayerAssistant
    {
        public static async Task<bool> IsPlayerAvailable() 
        {
            var wait = (int)Math.Round(1000 * Random.Shared.NextSingle());
            await Task.Delay( wait );
            return Random.Shared.NextSingle() < 0.2;
        }
    }
}