using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.DeleteActivityById;

public sealed record DeleteActivityByIdCommand(int Id,
    int ActivityId) : ICommand;
