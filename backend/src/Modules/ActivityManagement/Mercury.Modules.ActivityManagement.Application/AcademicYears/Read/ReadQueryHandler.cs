using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.Read;

public sealed class ReadQueryHandler(ActivityManagementDbContext dbContext)
    : IQueryHandler<ReadQuery, (IReadOnlyCollection<AcademicYearDto.Read>, int)>
{
    public async Task<(IReadOnlyCollection<AcademicYearDto.Read>, int)> Handle(ReadQuery query,
        CancellationToken cancellationToken)
    {
        IQueryable<AcademicYear> academicYears = dbContext.AcademicYears;

        if (!string.IsNullOrWhiteSpace(query.Searchterm))
            academicYears = academicYears.Where(ay =>
                EF.Functions.ILike(ay.StartDate.Year.ToString(), $"%{query.Searchterm}%") ||
                EF.Functions.ILike(ay.EndDate.Year.ToString(), $"%{query.Searchterm}%"));

        int count = await academicYears.CountAsync(cancellationToken);

        return (await academicYears
                .OrderByDescending(ay => ay.StartDate)
                .Select(ay => new AcademicYearDto.Read(ay.Id,
                    ay.Name,
                    ay.StartDate,
                    ay.EndDate,
                    ay.Activities.Count))
                .ToListAsync(cancellationToken),
            count);
    }
}
