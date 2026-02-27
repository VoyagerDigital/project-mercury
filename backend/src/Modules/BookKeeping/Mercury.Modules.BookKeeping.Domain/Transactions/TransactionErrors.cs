namespace Mercury.Modules.BookKeeping.Domain.Transactions;

public static class TransactionErrors
{
    public static readonly string AmountNegativeOrZero = $"{nameof(Transaction)}.{nameof(Transaction.Amount)}.NegativeOrZero";
    public static readonly string DescriptionEmpty = $"{nameof(Transaction)}.{nameof(Transaction.Description)}.Empty";
    public static readonly string DescriptionTooLong = $"{nameof(Transaction)}.{nameof(Transaction.Description)}.TooLong";
    public static readonly string TypeInvalid = $"{nameof(Transaction)}.{nameof(Transaction.Type)}.Invalid";
}