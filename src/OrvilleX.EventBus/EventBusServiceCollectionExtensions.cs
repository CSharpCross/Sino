using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrvilleX.EventBus;
using OrvilleX.EventBus.Channel;
using OrvilleX.EventBus.Common;
using OrvilleX.EventBus.Configuration;
using OrvilleX.EventBus.Consumer;
using OrvilleX.EventBus.Operations;
using OrvilleX.EventBus.Serialization;
using RabbitMQ.Client;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationSection section)
        {
            var mainCfg = new RabbitMqConfiguration();
            section.Bind(mainCfg);
            services.AddSingleton(mainCfg);

            services
                .AddSingleton<IConnectionFactory, ConnectionFactory>(provider =>
                {
                    var cfg = provider.GetService<RabbitMqConfiguration>();
                    return new ConnectionFactory
                    {
                        VirtualHost = cfg.VirtualHost,
                        UserName = cfg.Username,
                        Password = cfg.Password,
                        Port = cfg.Port,
                        HostName = cfg.Hostnames.FirstOrDefault() ?? string.Empty,
                        AutomaticRecoveryEnabled = cfg.AutomaticRecovery,
                        TopologyRecoveryEnabled = cfg.TopologyRecovery,
                        NetworkRecoveryInterval = cfg.RecoveryInterval,
                        ClientProperties = provider.GetService<IClientPropertyProvider>().GetClientProperties(cfg),
                        Ssl = cfg.Ssl
                    };
                })
                .AddSingleton<IClientPropertyProvider, ClientPropertyProvider>()
                .AddSingleton<IMessageSerializer, JsonMessageSerializer>()
                .AddSingleton<IBasicPropertiesProvider, BasicPropertiesProvider>()
                .AddSingleton<IChannelFactory, ChannelFactory>()
                .AddSingleton(c => ChannelFactoryConfiguration.Default)
                .AddSingleton<ITopologyProvider, TopologyProvider>()
                .AddTransient<IConfigurationEvaluator, ConfigurationEvaluator>()
                .AddTransient<IConsumerFactory, EventingBasicConsumerFactory>()
                .AddSingleton<ISubscriber, Subscriber>()
                .AddTransient<IPublishAcknowledger, PublishAcknowledger>(
                    p => new PublishAcknowledger(p.GetService<RabbitMqConfiguration>().PublishConfirmTimeout,p.GetService<ILogger<PublishAcknowledger>>())
                )
                .AddSingleton<INamingConventions, NamingConventions>()
                .AddTransient<IPublisher, Publisher>()
                .AddSingleton<IEventBus>(provider =>
                {
                    return ActivatorUtilities.CreateInstance<EventBus>(provider);
                });

            return services;
        }

        public static IApplicationBuilder AddHandler<TEvent>(this IApplicationBuilder app, Action<ISubscriptionConfigurationBuilder> configuration = null)
            where TEvent : IAsyncNotification
        {
            var eventBus = app.ApplicationServices.GetService<IEventBus>();

            eventBus.SubscribeAsync<TEvent>(async x =>
            {
                var handlers = app.ApplicationServices.GetServices<IAsyncNotificationHandler<TEvent>>();
                foreach (var handler in handlers)
                {
                    await handler.Handle(x);
                }
            }, configuration);

            return app;
        }
    }
}
