using Mercury.Modules.ActivityManagement.Application.Activities;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears;

public static class AcademicYearDto
{
    public sealed record Read(int Id,
        string Name,
        DateOnly StartDate,
        DateOnly EndDate,
        int ActivitiesCount);

    public sealed record ReadById(int Id,
        string Name,
        DateOnly StartDate,
        DateOnly EndDate,
        IReadOnlyCollection<ActivityDto.Read> Activities);
}
