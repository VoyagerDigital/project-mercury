namespace Mercury.Shared.Application.Messaging;

public interface IBaseQuery;

public interface IQuery : IBaseQuery;

public interface IQuery<TResponse> : IBaseQuery;