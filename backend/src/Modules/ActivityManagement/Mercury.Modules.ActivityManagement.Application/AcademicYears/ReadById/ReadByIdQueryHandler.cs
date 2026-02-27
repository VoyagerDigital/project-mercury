using FluentResults;
using Mercury.Modules.ActivityManagement.Application.Activities;
using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.ReadById;

public sealed class ReadByIdQueryHandler(ActivityManagementDbContext dbContext)
    : IQueryHandler<ReadByIdQuery, Result<AcademicYearDto.ReadById>>
{
    public async Task<Result<AcademicYearDto.ReadById>> Handle(ReadByIdQuery query,
        CancellationToken cancellationToken)
    {
        AcademicYear? academicYear = await dbContext.AcademicYears
            .Include(ay => ay.Activities)
            .FirstOrDefaultAsync(ay => ay.Id == query.Id,
                cancellationToken);

        if (academicYear is null)
            return Result.Fail<AcademicYearDto.ReadById>(new Error.NotFound<AcademicYear>());

        return Result.Ok(new AcademicYearDto.ReadById(academicYear.Id,
            academicYear.Name,
            academicYear.StartDate,
            academicYear.EndDate,
            academicYear.Activities
                .OrderBy(a => a.StartTime)
                .Select(a => new ActivityDto.Read(a.Id,
                    a.Name,
                    a.Description,
                    a.StartTime,
                    a.EndTime))
                .ToList()));
    }
}
