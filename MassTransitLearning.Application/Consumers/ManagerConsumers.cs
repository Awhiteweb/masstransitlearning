using MassTransit;
using Microsoft.Extensions.Logging;
using MassTransitLearning.Application.Events;

namespace MassTransitLearning.Application.Consumers
{
    public class ManagerConsumer(ILogger<ManagerConsumer> logger) : IConsumer<ManagerRequest>
    {
        public async Task Consume(ConsumeContext<ManagerRequest> context)
        {
            logger.LogInformation("Checking if Manager is available");
            if( !await ManagerAssistant.IsManagerAvailable() )
            {
                throw new ManagerUnavailableException("Manager");
            }
            logger.LogInformation("Manager is available");
            await context.Publish<ManagerResponse>(new {context.Message.CorrelationId});
        }
    }
    public class ManagerTwoConsumer(ILogger<ManagerTwoConsumer> logger) : IConsumer<ManagerAssistantRequest>
    {
        public async Task Consume(ConsumeContext<ManagerAssistantRequest> context)
        {
            logger.LogInformation("Checking if Managers assistant is available");
            if( !await ManagerAssistant.IsManagerAvailable() )
            {
                throw new ManagerUnavailableException("Managers assistant");
            }
            logger.LogInformation("Managers assistant is available");
            await context.Publish<ManagerAssistantResponse>(new {context.Message.CorrelationId});
        }
    }

    public class ManagerUnavailableException(string manager) : Exception( $"{manager} is unavailable") { }

    public static class ManagerAssistant
    {
        public static async Task<bool> IsManagerAvailable() 
        {
            var wait = (int)Math.Round(1000 * Random.Shared.NextSingle());
            await Task.Delay( wait );
            return Random.Shared.NextSingle() < 0.2;
        }
    }
}