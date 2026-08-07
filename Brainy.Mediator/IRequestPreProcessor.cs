namespace Brainy.Mediator;

public interface IRequestPreProcessor<in TRequest>
{
    Task ProcessAsync(TRequest request, CancellationToken cancellationToken);
}
