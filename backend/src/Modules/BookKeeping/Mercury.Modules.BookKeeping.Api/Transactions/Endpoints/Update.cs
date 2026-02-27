using FastEndpoints;
using FluentResults;
using Mercury.Modules.BookKeeping.Application.Transactions.UpdateById;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.BookKeeping.Application.Transactions.UpdateById.UpdateByIdCommand, FluentResults.Result>;

namespace Mercury.Modules.BookKeeping.Endpoints.Transactions.Endpoints;

public sealed class Update(ICommandHandler handler) : Endpoint<TransactionRequest.Update>
{
    public override void Configure()
    {
        Put("transaction/{id:int}");
        Version(1);
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(TransactionRequest.Update req, CancellationToken ct)
    {
        int id = Route<int>("id");
        
        UpdateByIdCommand command = new(id,
            req.Date,
            req.Description,
            req.Amount,
            req.Type);
        
        Result result = await handler.Handle(command, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;

        await Send.OkAsync(cancellation: ct);
    }
}