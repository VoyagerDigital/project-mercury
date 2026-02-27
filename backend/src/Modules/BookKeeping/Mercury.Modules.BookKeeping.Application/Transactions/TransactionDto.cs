using Mercury.Modules.BookKeeping.Domain.Transactions;

namespace Mercury.Modules.BookKeeping.Application.Transactions;

public static class TransactionDto
{
    public sealed record Read(int Id,
        string Description,
        decimal Amount,
        DateOnly Date,
        TransactionType TransactionType);
    
    public sealed record ReadById(int Id,
        string Description,
        decimal Amount,
        DateOnly Date,
        TransactionType TransactionType);
}