using Mercury.Modules.ActivityManagement.Domain.Activities;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Mercury.Modules.ActivityManagement.Application.Activities.Read;

public sealed class ReadQueryHandler(ActivityManagementDbContext dbContext)
    : IQueryHandler<ReadQuery, (IReadOnlyCollection<ActivityDto.Read>, int)>
{
    public async Task<(IReadOnlyCollection<ActivityDto.Read>, int)> Handle(ReadQuery query,
        CancellationToken cancellationToken)
    {
        IQueryable<Activity> activities = dbContext.Activities;

        if (!string.IsNullOrWhiteSpace(query.Searchterm))
            activities = activities.Where(a =>
                EF.Functions.ILike(a.Name, $"%{query.Searchterm}%") ||
                (a.Description != null && EF.Functions.ILike(a.Description, $"%{query.Searchterm}%")));

        int count = await activities.CountAsync(cancellationToken);

        return (await activities
                .OrderBy(a => a.StartTime)
                .Select(a => new ActivityDto.Read(a.Id,
                    a.Name,
                    a.Description,
                    a.StartTime,
                    a.EndTime))
                .ToListAsync(cancellationToken),
            count);
    }
}
