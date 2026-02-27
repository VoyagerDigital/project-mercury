using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.Activities;
using Mercury.Shared.Kernel.Entities;
using Mercury.Shared.Kernel.Extensions;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.ActivityManagement.Domain.AcademicYears;

public sealed class AcademicYear : Entity
{
    private List<Activity> _activities = new();
    
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    
    public string Name => $"{StartDate.Year} - {EndDate.Year}";
    public IReadOnlyCollection<Activity> Activities => _activities;

    private AcademicYear(DateOnly startDate,
        DateOnly endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Result<AcademicYear> Create(DateOnly startDate,
        DateOnly endDate)
    {
        Result validationResult = Validate(startDate,
            endDate);
        
        if (validationResult.IsFailed)
            return validationResult;
        
        AcademicYear academicYear = new(startDate,
            endDate);
        
        academicYear.RaiseDomainEvent(new AcademicYearEvents.Created());
        
        return Result.Ok(academicYear);
    }

    public Result Update(DateOnly startDate,
        DateOnly endDate)
    {
        Result validationResult = Validate(startDate,
            endDate);
        
        if (validationResult.IsFailed)
            return validationResult;
        
        StartDate = startDate;
        EndDate = endDate;
        
        RaiseDomainEvent(new AcademicYearEvents.Updated());
        
        return Result.Ok();
    }

    public Result<Activity> AddActivity(string name,
        string? description,
        DateTimeOffset startTime,
        DateTimeOffset endTime)
    {
        Result<Activity> creationResult = Activity.Create(name,
            description,
            startTime,
            endTime);
        
        if (creationResult.IsFailed)
            return creationResult;
        
        Activity activity = creationResult.Value;
        
        _activities.Add(activity);
        
        return Result.Ok(activity);
    }

    public Result DeleteActivity(int id)
    {
        Activity? activity = _activities.FirstOrDefault(a => a.Id == id);
        
        if (activity is null)
            return Result.Fail(new Error.NotFound<Activity>());

        activity.Delete();
        
        return Result.Ok();
    }
    
    private static Result Validate(DateOnly startDate,
        DateOnly endDate)
    {
        AcademicYear academicYear = new(startDate,
            endDate);
        
        AcademicYearValidator validator = new();

        return validator.Validate(academicYear)
            .ToDomainResult();
    }

    public override void Delete()
    {
        base.Delete();
        
        RaiseDomainEvent(new AcademicYearEvents.Deleted());
    }
}