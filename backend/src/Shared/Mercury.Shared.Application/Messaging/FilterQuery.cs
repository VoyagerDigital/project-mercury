namespace Mercury.Shared.Application.Messaging;

public abstract record BaseFilterQuery(
    string? Searchterm,
    int Page = 1,
    int PageSize = 10);

public abstract record FilterQuery(string? Searchterm,
    int Page = 1,
    int PageSize = 10) 
    : BaseFilterQuery(Searchterm,
        Page,
        PageSize), IQuery;

public abstract record FilterQuery<TResponse>(string? Searchterm,
    int Page = 1,
    int PageSize = 10) : BaseFilterQuery(Searchterm,
        Page,
        PageSize), IQuery<TResponse>;