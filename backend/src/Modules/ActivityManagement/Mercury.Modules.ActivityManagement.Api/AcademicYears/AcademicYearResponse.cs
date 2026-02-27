using Mercury.Modules.ActivityManagement.Application.AcademicYears;

namespace Mercury.Modules.ActivityManagement.Endpoints.AcademicYears;

public static class AcademicYearResponse
{
    public sealed record Read(IReadOnlyCollection<AcademicYearDto.Read> AcademicYears,
        int TotalCount);

    public sealed record ReadById(AcademicYearDto.ReadById AcademicYear);
}
