namespace DomainDrivenVerticalSlices.Template.Common.Tests.Mediator;

using DomainDrivenVerticalSlices.Template.Common.Mediator;

public class MediatorTests
{
    [Fact]
    public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Mediator(null!));
    }

    [Fact]
    public async Task Publish_NotificationTypedAsBaseInterface_DispatchesToConcreteHandler()
    {
        // Regression test: domain events flow out of BaseEntity.DomainEvents statically typed
        // as INotification, so Publish must resolve handlers by the runtime type.
        var handler = new RecordingNotificationHandler();
        var provider = new TestServiceProvider();
        provider.Register(
            typeof(IEnumerable<INotificationHandler<TestNotification>>),
            new INotificationHandler<TestNotification>[] { handler });
        var mediator = new Mediator(provider);

        INotification notification = new TestNotification();
        await mediator.Publish(notification);

        var received = Assert.Single(handler.Received);
        Assert.Same(notification, received);
    }

    [Fact]
    public async Task Publish_MultipleHandlers_InvokesAllHandlers()
    {
        var handler1 = new RecordingNotificationHandler();
        var handler2 = new RecordingNotificationHandler();
        var provider = new TestServiceProvider();
        provider.Register(
            typeof(IEnumerable<INotificationHandler<TestNotification>>),
            new INotificationHandler<TestNotification>[] { handler1, handler2 });
        var mediator = new Mediator(provider);

        await mediator.Publish(new TestNotification());

        Assert.Single(handler1.Received);
        Assert.Single(handler2.Received);
    }

    [Fact]
    public async Task Publish_NoRegisteredHandlers_DoesNotThrow()
    {
        var mediator = new Mediator(new TestServiceProvider());

        await mediator.Publish(new TestNotification());
    }

    [Fact]
    public async Task Publish_NullNotification_ThrowsArgumentNullException()
    {
        var mediator = new Mediator(new TestServiceProvider());

        await Assert.ThrowsAsync<ArgumentNullException>(() => mediator.Publish<TestNotification>(null!));
    }

    [Fact]
    public async Task Send_RegisteredHandler_ReturnsHandlerResponse()
    {
        var provider = new TestServiceProvider();
        provider.Register(typeof(IRequestHandler<TestRequest, string>), new TestRequestHandler());
        var mediator = new Mediator(provider);

        var response = await mediator.Send(new TestRequest());

        Assert.Equal("handled", response);
    }

    [Fact]
    public async Task Send_NoRegisteredHandler_ThrowsInvalidOperationException()
    {
        var mediator = new Mediator(new TestServiceProvider());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => mediator.Send(new TestRequest()));

        Assert.Contains(nameof(TestRequest), exception.Message);
    }

    [Fact]
    public async Task Send_PipelineBehaviors_RunInRegistrationOrderAroundHandler()
    {
        var executionOrder = new List<string>();
        var provider = new TestServiceProvider();
        provider.Register(
            typeof(IRequestHandler<TestRequest, string>),
            new TestRequestHandler(() => executionOrder.Add("handler")));
        provider.Register(
            typeof(IEnumerable<IPipelineBehavior<TestRequest, string>>),
            new IPipelineBehavior<TestRequest, string>[]
            {
                new RecordingBehavior("first", executionOrder),
                new RecordingBehavior("second", executionOrder),
            });
        var mediator = new Mediator(provider);

        await mediator.Send(new TestRequest());

        Assert.Equal(
            ["first:before", "second:before", "handler", "second:after", "first:after"],
            executionOrder);
    }

    private sealed record TestNotification : INotification;

    private sealed record TestRequest : IRequest<string>;

    private sealed class RecordingNotificationHandler : INotificationHandler<TestNotification>
    {
        public List<INotification> Received { get; } = [];

        public Task Handle(TestNotification notification, CancellationToken cancellationToken)
        {
            Received.Add(notification);
            return Task.CompletedTask;
        }
    }

    private sealed class TestRequestHandler(Action? onHandled = null) : IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            onHandled?.Invoke();
            return Task.FromResult("handled");
        }
    }

    private sealed class RecordingBehavior(string name, List<string> executionOrder) : IPipelineBehavior<TestRequest, string>
    {
        public async Task<string> Handle(TestRequest request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
        {
            executionOrder.Add($"{name}:before");
            var response = await next();
            executionOrder.Add($"{name}:after");
            return response;
        }
    }

    private sealed class TestServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services = [];

        public void Register(Type serviceType, object instance)
        {
            _services[serviceType] = instance;
        }

        public object? GetService(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out var service))
            {
                return service;
            }

            // Mirror Microsoft DI: IEnumerable<T> always resolves (empty when nothing is registered).
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return Array.CreateInstance(serviceType.GetGenericArguments()[0], 0);
            }

            return null;
        }
    }
}
