using FluentResults;

namespace Mercury.Shared.Application.Messaging;

public interface IQueryHandler<in TQuery>
    where TQuery : IQuery
{
    Task<Result> Handle(TQuery query,
        CancellationToken cancellationToken);
}

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery
{
    Task<TResponse> Handle(TQuery query,
        CancellationToken cancellationToken);
}