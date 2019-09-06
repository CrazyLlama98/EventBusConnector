using System.Threading.Tasks;

namespace EventBusConnector.Interfaces
{
    public interface IEventHandler
    {
        Task HandleAsync(string payload);
    }

    public interface IEventHandler<TEvent> : IEventHandler
    {
        Task HandleAsync(TEvent @event);
    }
}
