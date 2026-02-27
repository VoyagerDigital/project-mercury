using FluentResults;
using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Mercury.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace Mercury.Modules.BookKeeping.Application.Transactions.DeleteById;

public sealed class DeleteByIdCommandHandler(BookKeepingDbContext dbContext) : ICommandHandler<DeleteByIdCommand, Result>
{
    public async Task<Result> Handle(DeleteByIdCommand command, CancellationToken cancellationToken)
    {
        Transaction? transaction = await dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == command.Id,
                cancellationToken);

        if (transaction is null)
            return Result.Fail(new Error.NotFound<Transaction>());
        
        transaction.Delete();
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}