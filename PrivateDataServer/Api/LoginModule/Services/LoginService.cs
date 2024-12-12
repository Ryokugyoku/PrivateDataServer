using PrivateDataServer.Api.LoginModule;
using PrivateDataServer.Api.LoginModule.Model;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Threading.Tasks;
namespace PrivateDataServer.Api.LoginModule;
public class LoginService : ILoginService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public LoginService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// ログイン処理
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.UserId, request.Password, request.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return new LoginResult { Success = true };
        }
        return new LoginResult { Success = false, ErrorMessage = "Invalid login attempt." };
    }

    /// <summary>
    /// ロールにユーザーを追加
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public async Task<IdentityResult> AddUserToRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"User with ID {userId} not found." });
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }

        return await _userManager.AddToRoleAsync(user, role);
    }
}