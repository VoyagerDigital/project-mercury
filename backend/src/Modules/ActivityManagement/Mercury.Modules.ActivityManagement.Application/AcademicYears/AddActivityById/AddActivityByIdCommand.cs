using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.AddActivityById;

public sealed record AddActivityByIdCommand(int Id,
    string Name,
    string? Description,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime) : ICommand;
