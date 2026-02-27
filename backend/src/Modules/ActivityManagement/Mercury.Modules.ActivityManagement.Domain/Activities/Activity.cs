using FluentResults;
using Mercury.Shared.Kernel.Entities;
using Mercury.Shared.Kernel.Extensions;

namespace Mercury.Modules.ActivityManagement.Domain.Activities;

public sealed class Activity : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }

    private Activity(string name,
        string? description,
        DateTimeOffset startTime,
        DateTimeOffset endTime)
    {
        Name = name;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Result<Activity> Create(string name,
        string? description,
        DateTimeOffset startTime,
        DateTimeOffset endTime)
    {
        Result validationResult = Validate(name,
            description,
            startTime,
            endTime);
        
        if (validationResult.IsFailed)
            return validationResult;
        
        Activity activity = new(name,
            description,
            startTime,
            endTime);
        
        activity.RaiseDomainEvent(new ActivityEvents.Created());
        
        return Result.Ok(activity);
    }
    
    public Result Update(string name,
        string? description,
        DateTimeOffset startTime,
        DateTimeOffset endTime)
    {
        Result validationResult = Validate(name,
            description,
            startTime,
            endTime);
        
        if (validationResult.IsFailed)
            return validationResult;

        Name = name;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        
        RaiseDomainEvent(new ActivityEvents.Updated());

        return Result.Ok();
    }
    
    private static Result Validate(string name,
        string? description,
        DateTimeOffset startTime,
        DateTimeOffset endTime)
    {
        Activity activity = new(name,
            description,
            startTime,
            endTime);

        ActivityValidator validator = new();

        return validator.Validate(activity)
            .ToDomainResult();
    }
    
    public override void Delete()
    {
        base.Delete();
        
        RaiseDomainEvent(new ActivityEvents.Deleted());
    }
}