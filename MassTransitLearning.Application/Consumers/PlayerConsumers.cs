using MassTransit;
using MassTransitLearning.Application.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MassTransitLearning.Application.Consumers
{
    public class PlayerOneConsumer(ILogger<PlayerOneConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player one is available, {CTX}", context.Message.CorrelationId);
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException(new PlayerUnavailable("Player one", context.Message.CorrelationId).ToString());
            }
            logger.LogInformation("Player one is available, {CTX}", context.Message.CorrelationId);
            await context.Publish<PlayerOneResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerTwoConsumer(ILogger<PlayerTwoConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player two is available, {CTX}", context.Message.CorrelationId);
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException(new PlayerUnavailable("Player two", context.Message.CorrelationId).ToString());
            }
            logger.LogInformation("Player two is available, {CTX}", context.Message.CorrelationId);
            await context.Publish<PlayerTwoResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerThreeConsumer(ILogger<PlayerThreeConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player three is available, {CTX}", context.Message.CorrelationId);
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException(new PlayerUnavailable("Player three", context.Message.CorrelationId).ToString());
            }
            logger.LogInformation("Player three is available, {CTX}", context.Message.CorrelationId);
            await context.Publish<PlayerThreeResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerFourConsumer(ILogger<PlayerFourConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player four is available, {CTX}", context.Message.CorrelationId);
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException(new PlayerUnavailable("Player four", context.Message.CorrelationId).ToString());
            }
            logger.LogInformation("Player four is available, {CTX}", context.Message.CorrelationId);
            await context.Publish<PlayerFourResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerFiveConsumer(ILogger<PlayerFiveConsumer> logger) : IConsumer<PlayerRequest>
    {
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if player five is available, {CTX}", context.Message.CorrelationId);
            if( !await PlayerAssistant.IsPlayerAvailable() )
            {
                throw new PlayerUnavailableException(new PlayerUnavailable("Player five", context.Message.CorrelationId).ToString());
            }
            logger.LogInformation("Player five is available, {CTX}", context.Message.CorrelationId);
            await context.Publish<PlayerFiveResponse>(new {context.Message.CorrelationId});
        }
    }

    public class PlayerUnavailableException(string message) : Exception(message) { }

    public record PlayerUnavailable(string Player, Guid CorrelationId)
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public static class PlayerAssistant
    {
        public static async Task<bool> IsPlayerAvailable() 
        {
            var wait = (int)Math.Round(1000 * Random.Shared.NextSingle());
            await Task.Delay(wait);
            return Random.Shared.NextSingle() > 0.2;
        }
    }
}