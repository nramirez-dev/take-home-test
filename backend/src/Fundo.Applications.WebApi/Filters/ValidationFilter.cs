using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Fundo.Shared.Exceptions;

namespace Fundo.Applications.WebApi.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            throw new ValidationException(errors);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}