using FastEndpoints;
using FluentResults;
using Mercury.Modules.ActivityManagement.Application.AcademicYears;
using Mercury.Modules.ActivityManagement.Application.AcademicYears.ReadById;
using Mercury.Shared.Application.Messaging;
using MercuryShared.Api.Extensions;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears.Endpoints;

public sealed class ReadById(IQueryHandler<ReadByIdQuery, Result<AcademicYearDto.ReadById>> handler) : EndpointWithoutRequest<AcademicYearResponse.ReadById>
{
    public override void Configure()
    {
        Get("academic-year/{id:int}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id");

        ReadByIdQuery query = new(id);

        Result<AcademicYearDto.ReadById> result = await handler.Handle(query, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;

        await Send.OkAsync(new AcademicYearResponse.ReadById(result.Value),
            ct);
    }
}
