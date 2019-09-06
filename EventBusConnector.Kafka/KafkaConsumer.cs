using Confluent.Kafka;
using EventBusConnector.Interfaces;
using System;
using System.Collections.Generic;

namespace EventBusConnector.Kafka
{
    public class KafkaConsumer : IConsumer
    {
        private bool disposedValue = false;
        protected ConsumerConfig Config { get; set; }
        protected Dictionary<string, IConsumer<Null, string>> Consumers { get; }

        public KafkaConsumer(ConsumerConfig config)
        {
            Config = config;
        }

        public string Consume(string subject)
        {
            var consumer = GetOrAddConsumer(subject);
            var result = consumer.Consume();

            return result.Value;
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
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var consumer in Consumers.Values)
                    {
                        consumer.Dispose();
                    }
                }

                disposedValue = true;
            }
        }
        ~KafkaConsumer()
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
