using FluentResults;
using Mercury.Shared.Kernel.Entities;
using Mercury.Shared.Kernel.Extensions;

namespace Mercury.Modules.BookKeeping.Domain.Transactions;

public sealed class Transaction : Entity
{
    public DateOnly Date { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    private Transaction(DateOnly date, string description, decimal amount, TransactionType type)
    {
        Date = date;
        Description = description;
        Amount = amount;
        Type = type;
    }
    
    public static Result<Transaction> Create(DateOnly date, string description, decimal amount, TransactionType type)
    {
        Result validationResult = Validate(date, description, amount, type);
        
        if (validationResult.IsFailed)
            return validationResult;
        
        Transaction transaction = new(date, description, amount, type);
        
        return Result.Ok(transaction);
    }

    public Result Update(DateOnly date, string description, decimal amount, TransactionType type)
    {
        Result validationResult = Validate(date, description, amount, type);
        
        if (validationResult.IsFailed)
            return validationResult;

        Date = date;
        Description = description;
        Amount = amount;
        Type = type;
        
        return Result.Ok();
    }

    private static Result Validate(DateOnly date, string description, decimal amount, TransactionType type)
    {
        Transaction transaction = new(date, description, amount, type);
        TransactionValidator validator = new();
        return validator.Validate(transaction)
            .ToDomainResult();
    }
}