using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using EventBusConnector.Interfaces;

namespace EventBusConnector.Kafka
{
    public class KafkaPublisher : IPublisher
    {
        private bool _disposedValue = false;
        protected IProducer<Null, string> Producer { get; }
        protected ProducerConfig Config { get; }

        public KafkaPublisher(ProducerConfig config)
        {
            Config = config;
            Producer = new ProducerBuilder<Null, string>(Config).Build();
        }

        public async Task SendAsync(string subject, string payload)
        {
            await Producer.ProduceAsync(subject, new Message<Null, string> { Value = payload });
        }

        public void Send(string subject, string payload)
        {
            Producer.Produce(subject, new Message<Null, string> { Value = payload });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Producer.Dispose();
                }

                _disposedValue = true;
            }
        }

        ~KafkaPublisher()
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
