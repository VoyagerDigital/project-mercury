using FastEndpoints;
using FluentResults;
using Mercury.Modules.ActivityManagement.Application.Activities.UpdateById;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.ActivityManagement.Application.Activities.UpdateById.UpdateByIdCommand, FluentResults.Result>;

namespace Mercury.Modules.ActivityManagement.Endpoints.Activities.Endpoints;

public sealed class Update(ICommandHandler handler) : Endpoint<ActivityRequest.Update>
{
    public override void Configure()
    {
        Put("activity/{id:int}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ActivityRequest.Update req,
        CancellationToken ct)
    {
        int id = Route<int>("id");

        UpdateByIdCommand command = new(id,
            req.Name,
            req.Description,
            req.StartTime,
            req.EndTime);

        Result result = await handler.Handle(command, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;

        await Send.OkAsync(cancellation: ct);
    }
}
