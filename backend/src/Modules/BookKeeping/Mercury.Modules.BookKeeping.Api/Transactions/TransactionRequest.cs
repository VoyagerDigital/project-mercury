using FastEndpoints;
using FluentValidation;
using Mercury.Modules.BookKeeping.Domain.Transactions;
using MercuryShared.Api.Requests;

namespace Mercury.Modules.BookKeeping.Endpoints.Transactions;

public static class TransactionRequest
{
    public abstract record Mutate(
        string Description,
        decimal Amount,
        DateOnly Date,
        TransactionType Type);

    public sealed record Create(
        string Description,
        decimal Amount,
        DateOnly Date,
        TransactionType Type)
        : Mutate(Description,
            Amount,
            Date,
            Type);
    
    public sealed record Read(string? Searchterm,
        int Page = 1,
        int PageSize = 10) 
        : FilterableRequest(Searchterm,
            Page,
            PageSize);

    public sealed record Update(
        string Description,
        decimal Amount,
        DateOnly Date,
        TransactionType Type)
        : Mutate(Description,
            Amount,
            Date,
            Type);
}