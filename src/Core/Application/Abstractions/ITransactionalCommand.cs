using MediatR;

namespace Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public interface ICommand : IRequest
{
}

public interface ITransactionalCommand : ICommand;

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
where TCommand : ICommand<TResponse>
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}

public interface ITransactionalCommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
where TCommand : ICommand<TResponse>
{
}

public interface ITransactionalCommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}
