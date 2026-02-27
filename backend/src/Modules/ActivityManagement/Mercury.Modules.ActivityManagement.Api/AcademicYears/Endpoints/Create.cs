using FastEndpoints;
using FluentResults;
using Mercury.Modules.ActivityManagement.Application.AcademicYears.Create;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.ActivityManagement.Application.AcademicYears.Create.CreateCommand, FluentResults.Result<int>>;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears.Endpoints;

public sealed class Create(ICommandHandler handler) : Endpoint<AcademicYearRequest.Create>
{
    public override void Configure()
    {
        Post("academic-year");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(AcademicYearRequest.Create req,
        CancellationToken ct)
    {
        CreateCommand command = new(req.StartDate,
            req.EndDate);

        Result<int> result = await handler.Handle(command, ct);

        bool applicationResultFailed = await this.SendIfFailedAsync(result,
            ct);

        if (applicationResultFailed)
            return;

        await Send.CreatedAtAsync<ReadById>(new { id = result.Value },
            cancellation: ct);
    }
}
