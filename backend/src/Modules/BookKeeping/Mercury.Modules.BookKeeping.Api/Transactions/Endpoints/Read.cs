using FastEndpoints;
using Mercury.Modules.BookKeeping.Application.Transactions;
using Mercury.Modules.BookKeeping.Application.Transactions.Read;
using Mercury.Shared.Application.Messaging;

namespace Mercury.Modules.BookKeeping.Endpoints.Transactions.Endpoints;

public sealed class Read(IQueryHandler<ReadQuery, (IReadOnlyCollection<TransactionDto.Read>, int)> handler) : Endpoint<TransactionRequest.Read, TransactionResponse.Read>
{
    public override void Configure()
    {
        Get("transaction");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(TransactionRequest.Read req, CancellationToken ct)
    {
        ReadQuery query = new(req.Searchterm,
            req.Page,
            req.PageSize);

        (IReadOnlyCollection<TransactionDto.Read> transactions, int count) = await handler.Handle(query, ct);

        await Send.OkAsync(new TransactionResponse.Read(transactions, count),
            ct);
    }
}