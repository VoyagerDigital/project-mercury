using Mercury.Modules.ActivityManagement.Application.Activities;

namespace Mercury.Modules.ActivityManagement.Endpoints.Activities;

public static class ActivityResponse
{
    public sealed record Read(IReadOnlyCollection<ActivityDto.Read> Activities,
        int TotalCount);

    public sealed record ReadById(ActivityDto.ReadById Activity);
}
