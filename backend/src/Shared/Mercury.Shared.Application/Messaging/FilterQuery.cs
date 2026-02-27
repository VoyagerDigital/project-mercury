namespace Mercury.Shared.Application.Messaging;

public abstract class BaseFilterQuery
{
    public string? Searchterm { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
};

public sealed class FilterQuery : BaseFilterQuery, IQuery;

public sealed class IFilterQuery<TResponse> : IBaseFilterQuery, IQuery<TResponse>;