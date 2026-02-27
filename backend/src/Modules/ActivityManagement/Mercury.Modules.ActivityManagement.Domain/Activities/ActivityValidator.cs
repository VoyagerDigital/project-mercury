using FluentValidation;

namespace Mercury.Modules.ActivityManagement.Domain.Activities;

public sealed class ActivityValidator : AbstractValidator<Activity>
{
    public ActivityValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty().WithErrorCode(ActivityErrors.NameEmpty)
            .MaximumLength(100).WithErrorCode(ActivityErrors.NameTooLong);
        
        RuleFor(a => a.Description)
            .NotEmpty().When(a => a.Description is not null).WithErrorCode(ActivityErrors.DescriptionEmpty)
            .MaximumLength(500).When(a => a.Description is not null).WithErrorCode(ActivityErrors.DescriptionTooLong);

        RuleFor(a => a.StartTime)
            .LessThan(DateTime.UtcNow).WithErrorCode(ActivityErrors.StartTimeInThePast);
        
        RuleFor(a => a.EndTime)
            .LessThanOrEqualTo(a => a.StartTime).WithErrorCode(ActivityErrors.EndTimeBeforeStartTime);
    }
}