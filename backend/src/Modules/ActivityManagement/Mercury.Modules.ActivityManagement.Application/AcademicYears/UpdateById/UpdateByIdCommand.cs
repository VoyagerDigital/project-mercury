using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.UpdateById;

public sealed record UpdateByIdCommand(int Id,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand;
