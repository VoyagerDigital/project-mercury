using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.BookKeeping.Application.Transactions.UpdateById;

public sealed record UpdateByIdCommand(int Id,
    DateOnly Date,
    string Description,
    decimal Amount,
    TransactionType TransactionType) : ICommand;