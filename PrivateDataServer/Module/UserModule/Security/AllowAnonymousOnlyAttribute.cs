namespace PrivateDataServer.Module.UserModule.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
public class AllowAnonymousOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User?.Identity?.IsAuthenticated == true)
        {
            context.Result = new ForbidResult();
        }
        base.OnActionExecuting(context);
    }
}