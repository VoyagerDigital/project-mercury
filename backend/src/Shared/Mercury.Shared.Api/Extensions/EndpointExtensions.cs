using FastEndpoints;
using FluentResults;
using FluentValidation.Results;
using Error = Mercury.Shared.Kernel.Errors.Error;

namespace MercuryShared.Api.Extensions;

public static class EndpointExtensions
{
    public static async Task<bool> SendIfFailedAsync(this IEndpoint endpoint,
        ResultBase result,
        CancellationToken ct)
    {
        if (result.IsSuccess)
            return false;

        int statusCode = result.Errors
            .Any(e => e is Error.NotFound)
            ? 404
            : 422;

        foreach (IError error in result.Errors)
        {
            endpoint.ValidationFailures
                .Add(new(error.Message,
                    error.Message));
        }

        await endpoint.HttpContext.Response
            .SendErrorsAsync(endpoint.ValidationFailures,
                statusCode,
                cancellation: ct);

        return true;
    }
}