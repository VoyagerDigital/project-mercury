using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.AddActivityById;

public sealed class AddActivityByIdCommandHandler(ActivityManagementDbContext dbContext)
    : ICommandHandler<AddActivityByIdCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddActivityByIdCommand command,
        CancellationToken cancellationToken)
    {
        AcademicYear? academicYear = await dbContext.AcademicYears
            .FirstOrDefaultAsync(ay => ay.Id == command.Id,
                cancellationToken);

        if (academicYear is null)
            return Result.Fail<int>(new Error.NotFound<AcademicYear>());

        Result<Domain.Activities.Activity> addResult = academicYear.AddActivity(command.Name,
            command.Description,
            command.StartTime,
            command.EndTime);

        if (addResult.IsFailed)
            return Result.Fail(addResult.Errors);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok(addResult.Value.Id);
    }
}
