using FluentResults;
using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.BookKeeping.Application.Transactions.UpdateById;

public sealed class UpdateByIdCommandHandler(BookKeepingDbContext dbContext) : ICommandHandler<UpdateByIdCommand, Result>
{
    public async Task<Result> Handle(UpdateByIdCommand command, CancellationToken cancellationToken)
    {
        Transaction? transaction = await dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == command.Id,
                cancellationToken);

        if (transaction is null)
            return Result.Fail(new Error.NotFound<Transaction>());

        Result updateResult = transaction.Update(command.Date,
            command.Description,
            command.Amount,
            command.TransactionType);
        
        if (updateResult.IsFailed)
            return Result.Fail(updateResult.Errors);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}