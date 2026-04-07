using GymClassBooking.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess) return new NoContentResult();

        return result.Error switch
        {
            NotFoundError e => new NotFoundObjectResult(CreateProblemDetails(e, 404)),
            ConflictError e => new ConflictObjectResult(CreateProblemDetails(e, 409)),
            ValidationError e => new BadRequestObjectResult(CreateProblemDetails(e, 400)),
            _ => new BadRequestObjectResult(CreateProblemDetails(result.Error, 400))
        };
    }

    public static IActionResult ToActionResult<TValue>(this Result<TValue> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : ((Result)result).ToActionResult();
    }

    private static ValidationProblemDetails CreateProblemDetails(Error error, int status)
    {
        var errors = new Dictionary<string, string[]>
                {
                    { error.Code.Split('.')[0].ToLower(), [error.Message] }
                };

        return new ValidationProblemDetails(errors)
        {
            Title = error.Code,
            Status = status
        };
    }
}