using FluentResults;
using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.BookKeeping.Application.Transactions.Create;

public sealed class CreateCommandHandler(BookKeepingDbContext dbContext) : ICommandHandler<CreateCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCommand command, CancellationToken cancellationToken)
    {
        Result<Transaction> creationResult = Transaction.Create(command.Date,
            command.Description,
            command.Amount,
            command.TransactionType);

        if (creationResult.IsFailed)
            return Result.Fail(creationResult.Errors);
        
        Transaction transaction = creationResult.Value;

        dbContext.Transactions
            .Add(creationResult.Value);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(transaction.Id);
    }
}