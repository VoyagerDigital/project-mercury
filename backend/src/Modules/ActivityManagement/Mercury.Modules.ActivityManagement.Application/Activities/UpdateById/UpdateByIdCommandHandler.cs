using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.Activities;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Application.Activities.UpdateById;

public sealed class UpdateByIdCommandHandler(ActivityManagementDbContext dbContext)
    : ICommandHandler<UpdateByIdCommand, Result>
{
    public async Task<Result> Handle(UpdateByIdCommand command,
        CancellationToken cancellationToken)
    {
        Activity? activity = await dbContext.Activities
            .FirstOrDefaultAsync(a => a.Id == command.Id,
                cancellationToken);

        if (activity is null)
            return Result.Fail(new Error.NotFound<Activity>());

        Result updateResult = activity.Update(command.Name,
            command.Description,
            command.StartTime,
            command.EndTime);

        if (updateResult.IsFailed)
            return Result.Fail(updateResult.Errors);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}