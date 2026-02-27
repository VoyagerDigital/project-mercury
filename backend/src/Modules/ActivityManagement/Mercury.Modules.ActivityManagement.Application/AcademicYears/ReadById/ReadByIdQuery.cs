using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.AcademicYears.ReadById;

public sealed record ReadByIdQuery(int Id) : IQuery;
