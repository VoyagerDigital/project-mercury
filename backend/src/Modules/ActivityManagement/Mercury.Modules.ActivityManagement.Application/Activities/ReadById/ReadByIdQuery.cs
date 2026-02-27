using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.ActivityManagement.Application.Activities.ReadById;

public sealed record ReadByIdQuery(int Id) : IQuery;
