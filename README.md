# EventBusConnector
[![Build Status](https://dev.azure.com/andreiriclea98-github/EventBusConnector/_apis/build/status/CrazyLlama98.EventBusConnector?branchName=master)](https://dev.azure.com/andreiriclea98-github/EventBusConnector/_build/latest?definitionId=1&branchName=master) [![codecov](https://codecov.io/gh/CrazyLlama98/EventBusConnector/branch/master/graph/badge.svg)](https://codecov.io/gh/CrazyLlama98/EventBusConnector) [![CodeFactor](https://www.codefactor.io/repository/github/crazyllama98/eventbusconnector/badge)](https://www.codefactor.io/repository/github/crazyllama98/eventbusconnector) ![GitHub](https://img.shields.io/github/license/CrazyLlama98/EventBusConnector)

EventBusConnector: ![Nuget](https://img.shields.io/nuget/dt/EventBusConnector) ![Nuget](https://img.shields.io/nuget/v/EventBusConnector)

EventBusConnector.Kafka: ![Nuget](https://img.shields.io/nuget/dt/EventBusConnector.Kafka) ![Nuget](https://img.shields.io/nuget/v/EventBusConnector.Kafka)

Represents a wrapper for various Event Bus streaming clients like Kafka.
 ## Features
 
 - default implementation and interfaces for generic wrapper client agnostic of the Event Bus products;
 - modular extensions for dependency injection registration;
 - fast replace of actual event bus clients libraries.
  
### Current supported event buses
1. Kafka
#### Upcoming
- RabbitMQ
- N.A.T.S.

## Examples
### D.I. registration sample
```c#
// this adds the IEventBus default implementation
services.AddEventBus();
// this adds the Kafka Connectors
services.AddKafka(
	producerOptions => 
	{
		producerOptions.BootstrapServers = "localhost:9092";
	},
	consumerOptions => 
	{
		consumerOptions.BootstrapServers = "localhost:9092";
        consumerOptions.GroupId = "test-consumer";
        consumerOptions.EnableAutoCommit = true;
	}
);
```

### Basic Usage example
We demo some of the functionalities using one ExampleEvent class and one ExampleEventHandler class.
```c#
public class ExampleEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class ExampleEventHandler : IEventHandler<ExampleEvent>
{
	public async Task HandleAsync(ExampleEvent @event)
	{
	    // just for demo is used Task.Run
	    // this should be async database calls or other cases
	    await Task.Run(() =>
	    {
	        Console.WriteLine(JsonConvert.SerializeObject(@event));
	    });
	}
}
```
 
#### Subscribe
```c#
public class ExampleService : IHostedService
{
    private readonly IEventBus _eventBus;

    public ExampleService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // generic event handler
        await _eventBus.SubscribeAsync(new ExampleEventHandler());

        // specify subject name
        await _eventBus.SubscribeAsync(new ExampleEventHandler(),
         "test-subject");
    }
}
```
#### Publish
```c#
public class ExampleConsumerService
{
    private readonly IEventBus _eventBus;

    public ExampleConsumerService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task SomeMethod()
    {
        // event class name as subject
        await _eventBus.PublishAsync(new ExampleEvent {Id = Guid.NewGuid(), Name = "test"});

        // customer subject
        await _eventBus.PublishAsync(new ExampleEvent {Id = Guid.NewGuid(), Name = "test"}, "test-subject");
    }
}
```
