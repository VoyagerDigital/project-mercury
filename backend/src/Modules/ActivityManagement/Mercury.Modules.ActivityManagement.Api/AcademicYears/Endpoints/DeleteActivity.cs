using FastEndpoints;
using FluentResults;
using Mercury.Modules.ActivityManagement.Application.AcademicYears.DeleteActivityById;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.ActivityManagement.Application.AcademicYears.DeleteActivityById.DeleteActivityByIdCommand, FluentResults.Result>;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears.Endpoints;

public sealed class DeleteActivity(ICommandHandler handler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("academic-year/{id:int}/activity/{activityId:int}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id");
        int activityId = Route<int>("activityId");

        DeleteActivityByIdCommand command = new(id,
            activityId);

        Result result = await handler.Handle(command, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;

        await Send.NoContentAsync(ct);
    }
}
