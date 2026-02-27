using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.Activities;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Application.Activities.ReadById;

public sealed class ReadByIdQueryHandler(ActivityManagementDbContext dbContext)
    : IQueryHandler<ReadByIdQuery, Result<ActivityDto.ReadById>>
{
    public async Task<Result<ActivityDto.ReadById>> Handle(ReadByIdQuery query,
        CancellationToken cancellationToken)
    {
        Activity? activity = await dbContext.Activities
            .FirstOrDefaultAsync(a => a.Id == query.Id,
                cancellationToken);

        if (activity is null)
            return Result.Fail<ActivityDto.ReadById>(new Error.NotFound<Activity>());

        return Result.Ok(new ActivityDto.ReadById(activity.Id,
            activity.Name,
            activity.Description,
            activity.StartTime,
            activity.EndTime));
    }
}
