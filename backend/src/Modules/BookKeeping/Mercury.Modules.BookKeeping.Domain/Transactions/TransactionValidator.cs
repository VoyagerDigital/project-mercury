using FluentValidation;

namespace Mercury.Modules.BookKeeping.Domain.Transactions;

public sealed class TransactionValidator : AbstractValidator<Transaction>
{
    public TransactionValidator()
    {
        RuleFor(t => t.Amount)
            .GreaterThan(0).WithErrorCode(TransactionErrors.AmountNegativeOrZero);
        
        RuleFor(t => t.Description)
            .NotEmpty().WithErrorCode(TransactionErrors.DescriptionEmpty)
            .MaximumLength(200).WithErrorCode(TransactionErrors.DescriptionTooLong);
        
        RuleFor(t => t.Type)
            .IsInEnum().WithErrorCode(TransactionErrors.TypeInvalid);
    }
}