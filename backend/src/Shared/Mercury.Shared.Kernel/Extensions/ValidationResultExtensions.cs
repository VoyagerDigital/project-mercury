using FluentResults;
using FluentValidation.Results;

namespace Mercury.Shared.Kernel.Extensions;

public static class ValidationResultExtensions
{
    public static Result ToDomainResult(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
            return Result.Ok();

        return Result.Fail(validationResult.Errors
            .Select(e => e.ErrorCode));
    }
}