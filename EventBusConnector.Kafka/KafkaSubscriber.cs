using Confluent.Kafka;
using EventBusConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventBusConnector.Kafka
{
    public class KafkaSubscriber : ISubscriber
    {
        private bool _disposedValue = false;
        protected ConsumerConfig Config { get; set; }
        protected Dictionary<string, IConsumer<Null, string>> Consumers { get; } = new Dictionary<string, IConsumer<Null, string>>();

        public KafkaSubscriber(ConsumerConfig config)
        {
            Config = config;
        }

        public async Task SubscribeAsync<TEvent>(string subject, IEventHandler<TEvent> eventHandler)
            where TEvent : class
        {
            var consumer = GetOrAddConsumer(subject);

            while (true)
            {
                var result = consumer.Consume();
                var @event = JsonConvert.DeserializeObject<TEvent>(result.Value);

                await eventHandler.HandleAsync(@event);
            }
        }

        public async Task SubscribeAsync(string subject, IEventHandler eventHandler)
        {
            var consumer = GetOrAddConsumer(subject);
            var result = consumer.Consume();
            
            await eventHandler.HandleAsync(result.Value);
        }

        private IConsumer<Null, string> GetOrAddConsumer(string subject)
        {
            if (Consumers.ContainsKey(subject))
            {
                return Consumers[subject];
            }

            var consumer = new ConsumerBuilder<Null, string>(Config).Build();
            consumer.Subscribe(subject);
            Consumers.Add(subject, consumer);

            return consumer;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var consumer in Consumers.Values)
                    {
                        consumer.Dispose();
                    }
                }

                _disposedValue = true;
            }
        }
        ~KafkaSubscriber()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
