using System;
using System.Threading.Tasks;
using EventBusConnector.Interfaces;
using Newtonsoft.Json;

namespace EventBusConnector.Implementation
{
    public class EventBus : IEventBus
    {
        private bool disposedValue = false;
        protected IPublisher Publisher { get; set; }
        protected IConsumer Consumer { get; set; }

        public EventBus(IPublisher publisher, IConsumer consumer)
        {
            Publisher = publisher;
            Consumer = consumer;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, string subject = null)
            where TEvent : class
        {
            var payload = JsonConvert.SerializeObject(@event);

            await Publisher.SendAsync(subject ?? nameof(@event), payload);
        }

        public async Task PublishAsync(string payload, string subject)
        {
            await Publisher.SendAsync(subject, payload);
        }

        public async Task SubscribeAsync(IEventHandler eventHandler, string subject)
        {
            var payload = Consumer.Consume(subject);

            await eventHandler.HandleAsync(payload);
        }

        public async Task SubscribeAsync<TEvent>(IEventHandler<TEvent> eventHandler, string subject = null)
        {
            var payload = Consumer.Consume(subject);
            var @event = JsonConvert.DeserializeObject<TEvent>(payload);

            await eventHandler.HandleAsync(@event);
        }

        public void Publish<TEvent>(TEvent @event, string subject = null) where TEvent : class
        {
            var payload = JsonConvert.SerializeObject(@event);

            Publisher.Send(subject ?? nameof(@event), payload);
        }

        public void Publish(string payload, string subject)
        {
            Publisher.Send(subject, payload);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Publisher.Dispose();
                    Consumer.Dispose();
                }

                disposedValue = true;
            }
        }

        ~EventBus()
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
