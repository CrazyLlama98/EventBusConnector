using Confluent.Kafka;
using EventBusConnector.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EventBusConnector.Kafka.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafka(
            this IServiceCollection services,
            Action<ProducerConfig> producerOptions = null,
            Action<ConsumerConfig> consumerOptions = null)
        {
            var producerConfig = new ProducerConfig();
            var consumerConfig = new ConsumerConfig();

            if (producerOptions != null)
            {
                producerOptions.Invoke(producerConfig);
            }

            if (consumerOptions != null)
            {
                consumerOptions.Invoke(consumerConfig);
            }

            services
                .AddSingleton(producerConfig)
                .AddSingleton(consumerConfig)
                .AddSingleton<IPublisher, KafkaPublisher>()
                .AddSingleton<IConsumer, KafkaConsumer>();

            return services;
        }
    }
}
