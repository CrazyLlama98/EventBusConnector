using System;

namespace EventBusConnector.Interfaces
{
    public interface IConsumer : IDisposable
    {
        string Consume(string subject);
    }
}
