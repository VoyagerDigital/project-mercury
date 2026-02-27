using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.DeleteById;

public sealed record DeleteByIdCommand(int Id) : ICommand;
