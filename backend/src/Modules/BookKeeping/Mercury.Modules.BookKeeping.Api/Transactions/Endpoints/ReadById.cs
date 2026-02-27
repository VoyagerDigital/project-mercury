using FastEndpoints;
using FluentResults;
using Mercury.Modules.BookKeeping.Application.Transactions;
using Mercury.Modules.BookKeeping.Application.Transactions.ReadById;
using Mercury.Shared.Application.Messaging;
using MercuryShared.Api.Extensions;

namespace Mercury.Modules.BookKeeping.Endpoints.Transactions.Endpoints;

public sealed class ReadById(IQueryHandler<ReadByIdQuery, Result<TransactionDto.ReadById>> handler) : EndpointWithoutRequest<TransactionResponse.ReadById>
{
    public override void Configure()
    {
        Get("transaction/{id:int}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id");

        ReadByIdQuery query = new(id);

        Result<TransactionDto.ReadById> result = await handler.Handle(query, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;
        
        await Send.OkAsync(new TransactionResponse.ReadById(result.Value),
            ct);
    }
}