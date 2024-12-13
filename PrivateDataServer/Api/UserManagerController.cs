using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateDataServer.Module.Request;
namespace PrivateDataServer.Api;

[ApiController]
[Route("api/[controller]")]
public class UserManagerController : ControllerBase
{
    private readonly IUserManagerService _userManagerService;

    public UserManagerController(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    /// <summary>
    /// ユーザ作成API
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("CreateUser")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userManagerService.CreateUserAsync(request);
        if (result.Succeeded)
        {
            return Ok(new { message = "User created successfully" });
        }
        return BadRequest(result.Errors);
    }
    
    /// <summary>
    /// ユーザ更新API
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("UpdateUser")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var result = await _userManagerService.UpdateUserAsync(request);
        if (result.Succeeded)
        {
            return Ok(new { message = "User updated successfully" });
        }
        return BadRequest(result.Errors);
    }

    /// <summary>
    /// ユーザ削除API
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete("DeleteUser")]
    [Authorize]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request)
    {
        var result = await _userManagerService.DeleteUserAsync(request);
        if (result.Succeeded)
        {
            return Ok(new { message = "User deleted successfully" });
        }
        return BadRequest(result.Errors);
    }
}