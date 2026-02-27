using FluentResults;

namespace Mercury.Shared.Application.Messaging;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task Handle(TCommand command,
        CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand
{
    Task<TResponse> Handle(TCommand command,
        CancellationToken cancellationToken);
}