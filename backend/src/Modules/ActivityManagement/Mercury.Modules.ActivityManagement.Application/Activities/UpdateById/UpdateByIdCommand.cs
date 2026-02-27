using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.Activities.UpdateById;

public sealed record UpdateByIdCommand(int Id,
    string Name,
    string? Description,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime) : ICommand;
