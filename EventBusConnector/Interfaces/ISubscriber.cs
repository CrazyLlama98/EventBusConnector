using System;
using System.Threading.Tasks;

namespace EventBusConnector.Interfaces
{
    public interface ISubscriber : IDisposable
    {
        Task SubscribeAsync<TEvent>(string subject, IEventHandler<TEvent> eventHandler)
            where TEvent : class;

        Task SubscribeAsync(string subject, IEventHandler eventHandler);
    }
}
