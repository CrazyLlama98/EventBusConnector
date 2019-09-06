using System;
using System.Threading.Tasks;

namespace EventBusConnector.Interfaces
{
    public interface IPublisher : IDisposable
    {
        Task SendAsync(string subject, string payload);

        void Send(string subject, string payload);
    }
}
