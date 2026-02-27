using FastEndpoints;
using FluentResults;
using Mercury.Modules.BookKeeping.Application.Transactions.Create;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.BookKeeping.Application.Transactions.Create.CreateCommand, FluentResults.Result<int>>;

namespace Mercury.Modules.BookKeeping.Endpoints.Transactions.Endpoints;

public sealed class Create(ICommandHandler handler) : Endpoint<TransactionRequest.Create>
{
    public override void Configure()
    {
        Post("transaction");
        Version(1);
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(TransactionRequest.Create req, CancellationToken ct)
    {
        CreateCommand command = new(req.Date,
            req.Description,
            req.Amount,
            req.Type);
        
        Result<int> result = await handler.Handle(command, ct);

        bool applicationResultFailed = await this.SendIfFailedAsync(result,
            ct);

        if (applicationResultFailed)
            return;

        await Send.CreatedAtAsync<ReadById>(new { id = result.Value },
            cancellation: ct);
    }
}