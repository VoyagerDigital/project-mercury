using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.Create;

public sealed record CreateCommand(DateOnly StartDate,
    DateOnly EndDate) : ICommand;