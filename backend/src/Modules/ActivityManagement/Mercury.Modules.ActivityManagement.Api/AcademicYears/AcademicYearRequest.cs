using MercuryShared.Api.Requests;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears;

public static class AcademicYearRequest
{
    public abstract record Mutate(DateOnly StartDate,
        DateOnly EndDate);

    public sealed record Create(DateOnly StartDate,
        DateOnly EndDate)
        : Mutate(StartDate,
            EndDate);

    public sealed record Read(string? Searchterm,
        int Page = 1,
        int PageSize = 10)
        : FilterableRequest(Searchterm,
            Page,
            PageSize);

    public sealed record Update(DateOnly StartDate,
        DateOnly EndDate)
        : Mutate(StartDate,
            EndDate);

    public sealed record AddActivity(string Name,
        string? Description,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);
}
