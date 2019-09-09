using System.Threading.Tasks;

namespace EventBusConnector.Interfaces
{
    public interface IEventHandler
    {
        Task HandleAsync(string payload);
    }

    public interface IEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}
