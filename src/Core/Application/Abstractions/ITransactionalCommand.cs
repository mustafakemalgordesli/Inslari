using MediatR;

namespace Application.Abstractions;

public interface ICommand<T> : IRequest<T>
{
}

public interface ITransactionalCommand<T> : ICommand<T>
{
}

public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
}

public interface ITransactionalCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : ITransactionalCommand<TResponse>
{
}