namespace Brainy.Mediator;

// Handler interface for subscribers
public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
    [Obsolete("Use HandleAsync instead.")]
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}
