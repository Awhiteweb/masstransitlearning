using MassTransit;
using MassTransitLearning.Application.Dtos;
using MassTransitLearning.Application.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MassTransitLearning.Application.Consumers
{
    public class PlayerOneConsumer(ILogger<PlayerOneConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerOne;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            await PlayerAssistant.IsAvailable<PlayerOneConsumer, PlayerRequest, PlayerOneResponse>(logger, _player, context);
        }
    }
    public class PlayerTwoConsumer(ILogger<PlayerTwoConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerTwo;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            await PlayerAssistant.IsAvailable<PlayerTwoConsumer, PlayerRequest, PlayerTwoResponse>(logger, _player, context);
        }
    }
    public class PlayerThreeConsumer(ILogger<PlayerThreeConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerThree;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            await PlayerAssistant.IsAvailable<PlayerThreeConsumer, PlayerRequest, PlayerThreeResponse>(logger, _player, context);
        }
    }
    public class PlayerFourConsumer(ILogger<PlayerFourConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerFour;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            await PlayerAssistant.IsAvailable<PlayerFourConsumer, PlayerRequest, PlayerFourResponse>(logger, _player, context);
        }
    }
    public class PlayerFiveConsumer(ILogger<PlayerFiveConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerFive;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            await PlayerAssistant.IsAvailable<PlayerFiveConsumer, PlayerRequest, PlayerFiveResponse>(logger, _player, context);
        }
    }

    public class PlayerUnavailableException(string message) : Exception(message) { }

    public record PlayerUnavailable(string Player, Guid CorrelationId)
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public static class PlayerAssistant
    {
        public static async Task IsAvailable<TConsumer,TRequest,TResponse>(
            ILogger<TConsumer> logger, 
            string player, 
            ConsumeContext<TRequest> context) 
        where TConsumer : class
        where TRequest : class, CorrelatedBy<Guid>
        where TResponse : class, CorrelatedBy<Guid>
        {
            logger.LogInformation("Checking if {PLAYER} is available, {CTX}", player, context.Message.CorrelationId);
            var wait = (int)Math.Round(1000 * Random.Shared.NextSingle());
            await Task.Delay(wait);
            if(!(Random.Shared.NextSingle() > 0.2))
            {
                throw new PlayerUnavailableException(new PlayerUnavailable(player, context.Message.CorrelationId).ToString());
            }
            logger.LogInformation("{PLAYER} is available, {CTX}", player, context.Message.CorrelationId);
            await context.Publish<TResponse>(new {context.Message.CorrelationId});
        }
    }
}