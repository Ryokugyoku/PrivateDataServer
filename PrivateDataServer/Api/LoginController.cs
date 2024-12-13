namespace PrivateDataServer.api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using PrivateDataServer.Module.UserModule.Interface;
using PrivateDataServer.Module.UserModule.Security;
using PrivateDataServer.Module.Request;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;

    private readonly ILoginService _loginService;
    

    /// <summary>
    /// Constructor
    /// ログインインターフェースを注入
    /// </summary>
    /// <param name="loginService"> DIで依存注入されたサービス</param>
    /// <param name="signInManager"> DIで依存注入されたサービス </param>
    public LoginController(ILoginService loginService, SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
        _loginService = loginService;
    }

    /// <summary>
    /// ログイン用API
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    [AllowAnonymousOnly]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        
        var result = await _loginService.LoginAsync(request);
        Log.Information("Login request received for " +request.UserId);
        return Ok(result);
    }

    /// <summary>
    /// ログアウト用API
    /// </summary>
    /// <returns></returns>
    [HttpPost("Logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Log.Information("User logged out.");
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out successfully" });
    }
}