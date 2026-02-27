using FastEndpoints;
using FluentResults;
using Mercury.Modules.ActivityManagement.Application.AcademicYears.UpdateById;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.ActivityManagement.Application.AcademicYears.UpdateById.UpdateByIdCommand, FluentResults.Result>;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears.Endpoints;

public sealed class Update(ICommandHandler handler) : Endpoint<AcademicYearRequest.Update>
{
    public override void Configure()
    {
        Put("academic-year/{id:int}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(AcademicYearRequest.Update req,
        CancellationToken ct)
    {
        int id = Route<int>("id");

        UpdateByIdCommand command = new(id,
            req.StartDate,
            req.EndDate);

        Result result = await handler.Handle(command, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;

        await Send.OkAsync(cancellation: ct);
    }
}
