using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.Activities.Read;

public sealed record ReadQuery(string? Searchterm,
    int Page = 1,
    int PageSize = 10)
    : FilterQuery(Searchterm,
        Page,
        PageSize);
