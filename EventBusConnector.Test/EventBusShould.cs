using System;
using System.Threading.Tasks;
using EventBusConnector.Implementation;
using EventBusConnector.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace EventBusConnector.Test
{
    public class EventBusShould
    {
        private readonly IEventBus _eventBus;
        private readonly Mock<IPublisher> _publisher;
        private readonly Mock<ISubscriber> _subscriber;
        private readonly Mock<IEventHandler> _eventHandler;
        private readonly Mock<IEventHandler<object>> _genericEventHandler;

        public EventBusShould()
        {
            _publisher = new Mock<IPublisher>();
            _subscriber = new Mock<ISubscriber>();
            _eventHandler = new Mock<IEventHandler>();
            _genericEventHandler = new Mock<IEventHandler<object>>();

            _eventBus = new EventBus(_publisher.Object, _subscriber.Object);
        }

        [Fact]
        public async Task CallPublisherWhenCallingPublishAsync()
        {
            // arrange
            var payload = Guid.NewGuid().ToString();
            var subject = Guid.NewGuid().ToString();

            // act
            await _eventBus.PublishAsync(payload, subject);

            // assert
            _publisher
                .Verify(publisher => publisher.SendAsync(subject, payload), Times.Once);
        }

        [Fact]
        public async Task CallPublisherWhenCallingGenericPublishAsync()
        {
            // arrange
            var @event = new { };
            var subject = Guid.NewGuid().ToString();

            // act
            await _eventBus.PublishAsync<object>(@event, subject);

            // assert
            _publisher
                .Verify(publisher => publisher.SendAsync(subject, It.Is<string>(matcher => matcher.Equals(JsonConvert.SerializeObject(@event)))), Times.Once);
        }

        [Fact]
        public void CallPublisherWhenCallingPublish()
        {
            // arrange
            var payload = Guid.NewGuid().ToString();
            var subject = Guid.NewGuid().ToString();

            // act
            _eventBus.Publish(payload, subject);

            // assert
            _publisher
                .Verify(publisher => publisher.Send(subject, payload), Times.Once);
        }

        [Fact]
        public void CallPublisherWhenCallingGenericPublish()
        {
            // arrange
            var @event = new { };
            var subject = Guid.NewGuid().ToString();

            // act
            _eventBus.Publish<object>(@event, subject);

            // assert
            _publisher
                .Verify(publisher => publisher.Send(subject, It.Is<string>(matcher => matcher.Equals(JsonConvert.SerializeObject(@event)))), Times.Once);
        }

        [Fact]
        public async Task CallSubscriberWhenCallingSubscribeAsync()
        {
            // arrange
            var subject = Guid.NewGuid().ToString();

            // act
            await _eventBus.SubscribeAsync(_eventHandler.Object, subject);

            // assert
            _subscriber
                .Verify(subscriber => subscriber.SubscribeAsync(subject, _eventHandler.Object), Times.Once);
        }

        [Fact]
        public async Task CallSubscriberWhenCallingGenericSubscribeAsync()
        {
            // arrange
            var subject = Guid.NewGuid().ToString();

            // act
            await _eventBus.SubscribeAsync(_genericEventHandler.Object, subject);

            // assert
            _subscriber
                .Verify(subscriber => subscriber.SubscribeAsync(subject, _genericEventHandler.Object), Times.Once);
        }
    }
}
