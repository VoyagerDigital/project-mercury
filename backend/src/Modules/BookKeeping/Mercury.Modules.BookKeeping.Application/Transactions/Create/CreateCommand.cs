using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.BookKeeping.Application.Transactions.Create;

public sealed record CreateCommand(DateOnly Date,
    string Description,
    decimal Amount,
    TransactionType TransactionType) : ICommand;