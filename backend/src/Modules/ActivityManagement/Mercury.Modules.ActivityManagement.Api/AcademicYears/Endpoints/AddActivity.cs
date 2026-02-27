using FastEndpoints;
using FluentResults;
using Mercury.Modules.ActivityManagement.Application.AcademicYears.AddActivityById;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.ActivityManagement.Application.AcademicYears.AddActivityById.AddActivityByIdCommand, FluentResults.Result<int>>;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears.Endpoints;

public sealed class AddActivity(ICommandHandler handler) : Endpoint<AcademicYearRequest.AddActivity>
{
    public override void Configure()
    {
        Post("academic-year/{id:int}/activity");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(AcademicYearRequest.AddActivity req,
        CancellationToken ct)
    {
        int id = Route<int>("id");

        AddActivityByIdCommand command = new(id,
            req.Name,
            req.Description,
            req.StartTime,
            req.EndTime);

        Result<int> result = await handler.Handle(command, ct);

        bool applicationResultFailed = await this.SendIfFailedAsync(result,
            ct);

        if (applicationResultFailed)
            return;

        await Send.CreatedAtAsync<Mercury.Modules.ActivityManagement.Endpoints.Activities.Endpoints.ReadById>(new { id = result.Value },
            cancellation: ct);
    }
}
