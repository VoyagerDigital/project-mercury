using FluentValidation;

namespace Mercury.Modules.ActivityManagement.Domain.AcademicYears;

public sealed class AcademicYearValidator : AbstractValidator<AcademicYear>
{
    public AcademicYearValidator()
    {
        RuleFor(x => x.StartDate)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithErrorCode(AcademicYearErrors.StartDateInThePast);
        
        RuleFor(x => x.EndDate)
            .LessThan(x => x.StartDate).WithErrorCode(AcademicYearErrors.EndDateBeforeStartTime);
    }
}