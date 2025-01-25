using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Selah.WebAPI.Extensions;

namespace Selah.WebAPI.Filters;

public class ValidAppRequestContextFilter: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var requestContext = context.HttpContext.Request.GetAppRequestContext();

        // Check if UserId is empty
        if (requestContext.UserId == Guid.Empty)
        {
            // Return Unauthorized result
            context.Result = new UnauthorizedResult();
        }

        base.OnActionExecuting(context);
    }
}