using App.Application;
using App.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.API.Filters;
public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepsitory)
    : Attribute,
    IAsyncActionFilter
    where T : class
    where TId : struct
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        {
            if (!context.ActionArguments.TryGetValue("id", out var idValue) || idValue is not TId id)
            {
                await next();
                return;
            }

            if (!await genericRepsitory.AnyAsync(id))
            {
                var entityName = typeof(T).Name;
                var actionName = context.ActionDescriptor.RouteValues["action"];
                var result = ServiceResult.Fail($"Entity {entityName} not found in {actionName}");
                context.Result = new NotFoundObjectResult(result);
                return;
            }

            await next();
        }

    }
}
