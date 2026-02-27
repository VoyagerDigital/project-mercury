using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.DeleteById;

public sealed class DeleteByIdCommandHandler(ActivityManagementDbContext dbContext)
    : ICommandHandler<DeleteByIdCommand, Result>
{
    public async Task<Result> Handle(DeleteByIdCommand command,
        CancellationToken cancellationToken)
    {
        AcademicYear? academicYear = await dbContext.AcademicYears
            .FirstOrDefaultAsync(ay => ay.Id == command.Id,
                cancellationToken);

        if (academicYear is null)
            return Result.Fail(new Error.NotFound<AcademicYear>());

        academicYear.Delete();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
