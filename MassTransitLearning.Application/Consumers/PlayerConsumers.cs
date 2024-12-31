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
            logger.LogInformation("Checking if {PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            await PlayerAssistant.Wait();
            logger.LogInformation("{PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            await context.Publish<PlayerOneResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerTwoConsumer(ILogger<PlayerTwoConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerTwo;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if {PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            await PlayerAssistant.Wait();
            throw new PlayerUnavailableException(new PlayerUnavailable(_player, context.Message.CorrelationId).ToString());
        }
    }
    public class PlayerThreeConsumer(ILogger<PlayerThreeConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerThree;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if {PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            await PlayerAssistant.Wait();
            logger.LogInformation("{PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            await context.Publish<PlayerThreeResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerFourConsumer(ILogger<PlayerFourConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerFour;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if {PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            await PlayerAssistant.Wait();
            logger.LogInformation("{PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            await context.Publish<PlayerFourResponse>(new {context.Message.CorrelationId});
        }
    }
    public class PlayerFiveConsumer(ILogger<PlayerFiveConsumer> logger) : IConsumer<PlayerRequest>
    {
        public static readonly string _player = Players.PlayerFive;
        public async Task Consume(ConsumeContext<PlayerRequest> context)
        {
            logger.LogInformation("Checking if {PLAYER} is available, {CTX}", _player, context.Message.CorrelationId);
            var wait = (int)Math.Round(10000 * Random.Shared.NextSingle());
            await Task.Delay(wait);
            throw new PlayerUnavailableException(new PlayerUnavailable(_player, context.Message.CorrelationId).ToString());
        }
    }

    public class PlayerUnavailableException(string message) : Exception(message) { }

    public record PlayerUnavailable(string Player, Guid CorrelationId)
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public static class PlayerAssistant
    {
        public static async Task Wait()
        {
            var wait = (int)Math.Round(1000 * Random.Shared.NextSingle());
            await Task.Delay(wait);
        }
    }
}