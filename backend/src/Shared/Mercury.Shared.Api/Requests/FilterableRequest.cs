namespace MercuryShared.Api.Requests;

public abstract record FilterableRequest(string? Searchterm,
    int Page = 1,
    int PageSize = 10);