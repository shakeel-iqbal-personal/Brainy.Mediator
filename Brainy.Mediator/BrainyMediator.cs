using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Threading.Tasks;

namespace Brainy.Mediator;

public class BrainyMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public BrainyMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        // 1. Resolve the main handler
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException($"No handler registered for {requestType.Name}");

        // 2. Resolve all pipeline behaviors registered for this request/response type
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = _serviceProvider.GetServices(behaviorType)
            .Cast<dynamic>()
            .Reverse(); // Reverse so the first registered behavior runs first

        // 3. Chain the execution delegates
        Func<Task<TResponse>> aggregatePipeline = () =>
        {
            var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle));
            return (Task<TResponse>)method!.Invoke(handler, new object[] { request, cancellationToken })!;
        };

        foreach (var behavior in behaviors)
        {
            var currentNext = aggregatePipeline;
            aggregatePipeline = () => behavior.HandleAsync((dynamic)request, currentNext, cancellationToken);
        }

        // 4. Run the entire chain
        return await aggregatePipeline();
    }


    public async Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        var notificationType = notification.GetType();

        // 1. Resolve all registered handlers for this specific notification type
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        var handlers = _serviceProvider.GetServices(handlerType);

        // 2. Prepare execution tasks
        var tasks = new List<Task>();
        var handleMethod = handlerType.GetMethod(nameof(INotificationHandler<INotification>.HandleAsync));

        foreach (var handler in handlers)
        {
            if (handler != null)
            {
                var task = (Task)handleMethod!.Invoke(handler, new object[] { notification, cancellationToken })!;
                tasks.Add(task);
            }
        }

        // 3. Broadcast to all handlers simultaneously
        await Task.WhenAll(tasks);
    }

    [Obsolete("Use SendAsync instead.")]    
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
       return SendAsync(request, cancellationToken);    
    }

    [Obsolete("Use PublishAsync instead.")] 
    public Task Publish(INotification notification, CancellationToken cancellationToken = default)
    {
        return PublishAsync(notification, cancellationToken);   
    }
}
