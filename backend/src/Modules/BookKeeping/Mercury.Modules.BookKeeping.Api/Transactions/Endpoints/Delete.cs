using FastEndpoints;
using FluentResults;
using Mercury.Modules.BookKeeping.Application.Transactions.DeleteById;
using MercuryShared.Api.Extensions;
using ICommandHandler = Mercury.Shared.Application.Messaging.ICommandHandler<Mercury.Modules.BookKeeping.Application.Transactions.DeleteById.DeleteByIdCommand, FluentResults.Result>;

namespace Mercury.Modules.BookKeeping.Endpoints.Transactions.Endpoints;

public sealed class Delete(ICommandHandler handler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("transaction/{id:int}");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        int id = Route<int>("id");

        DeleteByIdCommand command = new(id);

        Result result = await handler.Handle(command, ct);

        bool isFailed = await this.SendIfFailedAsync(result,
            ct);

        if (isFailed)
            return;
        
        await Send.NoContentAsync(ct);
    }
}