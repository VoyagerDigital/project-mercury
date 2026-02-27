using FastEndpoints;
using Mercury.Modules.ActivityManagement.Application.AcademicYears;
using Mercury.Modules.ActivityManagement.Application.AcademicYears.Read;
using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears.Endpoints;

public sealed class Read(IQueryHandler<ReadQuery, (IReadOnlyCollection<AcademicYearDto.Read>, int)> handler) : Endpoint<AcademicYearRequest.Read, AcademicYearResponse.Read>
{
    public override void Configure()
    {
        Get("academic-year");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(AcademicYearRequest.Read req,
        CancellationToken ct)
    {
        ReadQuery query = new(req.Searchterm,
            req.Page,
            req.PageSize);

        (IReadOnlyCollection<AcademicYearDto.Read> academicYears, int count) = await handler.Handle(query, ct);

        await Send.OkAsync(new AcademicYearResponse.Read(academicYears, count),
            ct);
    }
}
