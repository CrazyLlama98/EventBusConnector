using System;
using System.Threading.Tasks;
using EventBusConnector.Interfaces;
using Newtonsoft.Json;

namespace EventBusConnector.Implementation
{
    public class EventBus : IEventBus
    {
        private bool _disposedValue = false;
        protected IPublisher Publisher { get; set; }
        protected ISubscriber Subscriber { get; set; }

        public EventBus(IPublisher publisher, ISubscriber subscriber)
        {
            Publisher = publisher;
            Subscriber = subscriber;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, string subject = null)
            where TEvent : class
        {
            var payload = JsonConvert.SerializeObject(@event);

            await Publisher.SendAsync(subject ?? nameof(TEvent), payload);
        }

        public async Task PublishAsync(string payload, string subject)
        {
            await Publisher.SendAsync(subject, payload);
        }

        public async Task SubscribeAsync(IEventHandler eventHandler, string subject)
        {
            await Subscriber.SubscribeAsync(subject, eventHandler);
        }

        public async Task SubscribeAsync<TEvent>(IEventHandler<TEvent> eventHandler, string subject = null)
            where TEvent : class
        {
            await Subscriber.SubscribeAsync(subject, eventHandler);
        }

        public void Publish<TEvent>(TEvent @event, string subject = null) where TEvent : class
        {
            var payload = JsonConvert.SerializeObject(@event);

            Publisher.Send(subject ?? nameof(TEvent), payload);
        }

        public void Publish(string payload, string subject)
        {
            Publisher.Send(subject, payload);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Publisher.Dispose();
                    Subscriber.Dispose();
                }

                _disposedValue = true;
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
