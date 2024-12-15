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
public class FileManagerController : ControllerBase{

    private readonly ILoginService _loginService;
    private readonly SignInManager<IdentityUser> _signInManager;

    FileManagerController(ILoginService loginService, SignInManager<IdentityUser> signInManager){
        _loginService = loginService;
        _signInManager = signInManager;
    }

    /// <summary>
    /// ファイルアップロード専用エンドポイント
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("UploadFile")]
    [Authorize]
    public async Task<IActionResult> UploadFile(IFormFile file){
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var filePath = Path.Combine("uploads", file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { filePath });
    }

}