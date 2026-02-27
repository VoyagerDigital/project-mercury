using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.BookKeeping.Application.Transactions.ReadById;

public sealed record ReadByIdQuery(int Id) : IQuery;