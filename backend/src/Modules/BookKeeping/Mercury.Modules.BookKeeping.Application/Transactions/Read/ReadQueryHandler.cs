using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Mercury.Modules.BookKeeping.Application.Transactions.Read;

public sealed class ReadQueryHandler(BookKeepingDbContext dbContext) : IQueryHandler<ReadQuery, (IReadOnlyCollection<TransactionDto.Read>, int)>
{
    public async Task<(IReadOnlyCollection<TransactionDto.Read>, int)> Handle(ReadQuery query, CancellationToken cancellationToken)
    {
        IQueryable<Transaction> transactions = dbContext.Transactions;
        
        int count = await transactions.CountAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(query.Searchterm))
            transactions = transactions.Where(t => 
                EF.Functions.ILike(t.Description, $"%{query.Searchterm}%"));

        return (await transactions.Select(t => new TransactionDto.Read(t.Id,
                    t.Description,
                    t.Amount,
                    t.Date,
                    t.Type))
                .ToListAsync(cancellationToken),
            count);
    }
}