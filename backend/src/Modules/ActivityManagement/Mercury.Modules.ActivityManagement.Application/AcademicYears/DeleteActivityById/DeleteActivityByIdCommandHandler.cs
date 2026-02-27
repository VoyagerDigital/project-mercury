using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.DeleteActivityById;

public sealed class DeleteActivityByIdCommandHandler(ActivityManagementDbContext dbContext)
    : ICommandHandler<DeleteActivityByIdCommand, Result>
{
    public async Task<Result> Handle(DeleteActivityByIdCommand command,
        CancellationToken cancellationToken)
    {
        AcademicYear? academicYear = await dbContext.AcademicYears
            .Include(ay => ay.Activities)
            .FirstOrDefaultAsync(ay => ay.Id == command.Id,
                cancellationToken);

        if (academicYear is null)
            return Result.Fail(new Error.NotFound<AcademicYear>());

        Result deleteResult = academicYear.DeleteActivity(command.ActivityId);

        if (deleteResult.IsFailed)
            return Result.Fail(deleteResult.Errors);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
