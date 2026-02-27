namespace Mercury.Modules.ActivityManagement.Application.Activities;

public static class ActivityDto
{
    public sealed record Read(int Id,
        string Name,
        string? Description,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);

    public sealed record ReadById(int Id,
        string Name,
        string? Description,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);
}
