using FluentResults;
using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.BookKeeping.Application.Transactions.ReadById;

public sealed class ReadByIdQueryHandler(BookKeepingDbContext dbContext) : IQueryHandler<ReadByIdQuery, Result<TransactionDto.ReadById>>
{
    public async Task<Result<TransactionDto.ReadById>> Handle(ReadByIdQuery query, CancellationToken cancellationToken)
    {
        Transaction? transaction = await dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == query.Id,
                cancellationToken);

        if (transaction is null)
            return Result.Fail<TransactionDto.ReadById>(new Error.NotFound<Transaction>());

        return Result.Ok(
            new TransactionDto.ReadById(transaction.Id,
                transaction.Description,
                transaction.Amount,
                transaction.Date,
                transaction.Type)
        );
    }
}