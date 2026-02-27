using FastEndpoints;
using Mercury.Modules.ActivityManagement.Application.Activities;
using Mercury.Modules.ActivityManagement.Application.Activities.Read;
using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Endpoints.Activities.Endpoints;

public sealed class Read(IQueryHandler<ReadQuery, (IReadOnlyCollection<ActivityDto.Read>, int)> handler) : Endpoint<ActivityRequest.Read, ActivityResponse.Read>
{
    public override void Configure()
    {
        Get("activity");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ActivityRequest.Read req,
        CancellationToken ct)
    {
        ReadQuery query = new(req.Searchterm,
            req.Page,
            req.PageSize);

        (IReadOnlyCollection<ActivityDto.Read> activities, int count) = await handler.Handle(query, ct);

        await Send.OkAsync(new ActivityResponse.Read(activities, count),
            ct);
    }
}
