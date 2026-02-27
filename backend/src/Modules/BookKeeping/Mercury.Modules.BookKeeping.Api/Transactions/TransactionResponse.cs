using Mercury.Modules.BookKeeping.Application.Transactions;
using Mercury.Modules.BookKeeping.Domain.Transactions;

namespace Mercury.Modules.BookKeeping.Endpoints.Transactions;

public static class TransactionResponse
{
    public sealed record Read(IReadOnlyCollection<TransactionDto.Read> Transactions,
        int TotalCount);
        
    public sealed record ReadById(TransactionDto.ReadById Transaction);
}