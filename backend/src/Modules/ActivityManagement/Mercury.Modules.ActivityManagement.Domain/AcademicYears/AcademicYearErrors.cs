namespace Mercury.Modules.ActivityManagement.Domain.AcademicYears;

public static class AcademicYearErrors
{
    public static readonly string StartDateInThePast = $"{nameof(AcademicYear)}.{nameof(AcademicYear.StartDate)}.InThePast";
    public static readonly string EndDateBeforeStartTime = $"{nameof(AcademicYear)}.{nameof(AcademicYear.EndDate)}.BeforeStartDate";
}