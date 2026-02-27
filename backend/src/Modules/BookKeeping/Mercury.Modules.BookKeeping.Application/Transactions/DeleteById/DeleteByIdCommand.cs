using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.BookKeeping.Application.Transactions.DeleteById;

public sealed record DeleteByIdCommand(int Id) : ICommand;