namespace Brainy.Mediator;

// The dispatcher interface
public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    // New Publish method for notifications
    Task PublishAsync(INotification notification, CancellationToken cancellationToken = default);

    [Obsolete("Use SendAsync instead.")]
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    // New Publish method for notifications
    [Obsolete("Use PublishAsync instead.")]
    Task Publish(INotification notification, CancellationToken cancellationToken = default);
}
