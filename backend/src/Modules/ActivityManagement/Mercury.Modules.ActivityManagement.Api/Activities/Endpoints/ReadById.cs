using FastEndpoints;
using FluentResults;
using Mercury.Modules.ActivityManagement.Application.Activities;
using Mercury.Modules.ActivityManagement.Application.Activities.ReadById;
using Mercury.Shared.Application.Messaging;
using MercuryShared.Api.Extensions;

namespace Mercury.Modules.ActivityManagement.Endpoints.Activities.Endpoints;

public sealed class ReadById(IQueryHandler<ReadByIdQuery, Result<ActivityDto.ReadById>> handler) : EndpointWithoutRequest<ActivityResponse.ReadById>
{
    public override void Configure()
    {
        Get("activity/{id:int}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id");

        ReadByIdQuery query = new(id);

        Result<ActivityDto.ReadById> result = await handler.Handle(query, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;

        await Send.OkAsync(new ActivityResponse.ReadById(result.Value),
            ct);
    }
}
