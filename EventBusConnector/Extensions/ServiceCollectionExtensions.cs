using EventBusConnector.Implementation;
using EventBusConnector.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusConnector.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>();

            return services;
        }
    }
}
