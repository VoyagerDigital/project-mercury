using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.UpdateById;

public sealed class UpdateByIdCommandHandler(ActivityManagementDbContext dbContext)
    : ICommandHandler<UpdateByIdCommand, Result>
{
    public async Task<Result> Handle(UpdateByIdCommand command,
        CancellationToken cancellationToken)
    {
        AcademicYear? academicYear = await dbContext.AcademicYears
            .FirstOrDefaultAsync(ay => ay.Id == command.Id,
                cancellationToken);

        if (academicYear is null)
            return Result.Fail(new Error.NotFound<AcademicYear>());

        Result updateResult = academicYear.Update(command.StartDate,
            command.EndDate);

        if (updateResult.IsFailed)
            return Result.Fail(updateResult.Errors);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}