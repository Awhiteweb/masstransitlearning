using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MassTransitLearning.Application.Consumers;
using MassTransitLearning.Application.Sagas;

namespace MassTransitLearning.Application
{
    public static class ProgramExtensions
    {
        public static void AddMassTransit(this IServiceCollection services)
        {
            var assemblies = new Assembly[] { typeof(ProgramExtensions).Assembly };
            services.AddMassTransit(config =>
            {
                config.AddSagaStateMachine<MatchBookingStateMachine, MatchBookingState>()
                    .MongoDbRepository(r =>
                    {
                        r.Connection = "mongodb://root:password@mongo/";
                        r.DatabaseName = "matchdb";
                    });
                config.AddSagas(assemblies);
                config.AddActivities(assemblies);
                config.AddConsumers(assemblies);

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("user");
                        h.Password("bitnami");
                    });
                    cfg.ConfigureEndpoints(ctx);
                    // cfg.UseMessageRetry(r => {
                    //     r.Interval(5, 1000);
                    //     r.Ignore(typeof(PlayerUnavailableException));
                    // });
                });
            });
        }
    }
}