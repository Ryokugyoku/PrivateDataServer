using Serilog.Context;
namespace PrivateDataServer.Module.LogModule;
/// <summary>
/// ユーザ情報をログに出力するミドルウェア
/// 
/// </summary>
public class ExecUserLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ExecUserLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string execUser;
        if(context.User.Identity.IsAuthenticated)
        {
            execUser = context.User.Identity.Name?? "SYSTEM";
        }
        else
        {
            execUser = "SYSTEM";
        }
 
        using (LogContext.PushProperty("ExecUser", execUser))
        {
            await _next(context);
        }
    }
}