using Microsoft.AspNetCore.Mvc.Filters;

namespace Selah.WebAPI.Filters;

public class DebugOnlyAsyncActionFilter :   Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
#if !DEBUG
      
        context.Result = new NotFoundResult();
        return;
#endif
        await next();

    }
}