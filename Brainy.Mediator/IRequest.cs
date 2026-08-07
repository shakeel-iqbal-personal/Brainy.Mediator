namespace Brainy.Mediator;

public interface IRequest : IRequest<Unit>  { }
public interface IRequest<TResponse> { }
