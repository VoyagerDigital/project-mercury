using FluentResults;
using Mercury.Modules.ActivityManagement.Domain.AcademicYears;
using Mercury.Modules.ActivityManagement.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.Create;

public sealed class CreateCommandHandler(ActivityManagementDbContext dbContext) : ICommandHandler<CreateCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        Result<AcademicYear> creationResult = AcademicYear.Create(command.StartDate,
            command.EndDate);

        if (creationResult.IsFailed)
            return Result.Fail(creationResult.Errors);
        
        AcademicYear academicYear = creationResult.Value;
        
        dbContext.AcademicYears
            .Add(academicYear);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(academicYear.Id);
    }
}