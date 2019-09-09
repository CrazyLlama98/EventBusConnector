using System;
using System.Threading.Tasks;

namespace EventBusConnector.Interfaces
{
    public interface IEventBus : IDisposable
    {
        Task PublishAsync<TEvent>(TEvent @event, string subject = null) 
            where TEvent : class;

        Task PublishAsync(string payload, string subject);

        void Publish<TEvent>(TEvent @event, string subject = null)
            where TEvent : class;

        void Publish(string payload, string subject);

        Task SubscribeAsync(IEventHandler eventHandler, string subject);

        Task SubscribeAsync<TEvent>(IEventHandler<TEvent> eventHandler, string subject = null)
            where TEvent : class;
    }
}
