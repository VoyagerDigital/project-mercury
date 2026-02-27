using MercuryShared.Api.Requests;

namespace Mercury.Modules.ActivityManagement.Endpoints.Activities;

public static class ActivityRequest
{
    public abstract record Mutate(string Name,
        string? Description,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);

    public sealed record Read(string? Searchterm,
        int Page = 1,
        int PageSize = 10)
        : FilterableRequest(Searchterm,
            Page,
            PageSize);

    public sealed record Update(string Name,
        string? Description,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime)
        : Mutate(Name,
            Description,
            StartTime,
            EndTime);
}
