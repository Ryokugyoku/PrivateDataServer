using Microsoft.AspNetCore.Identity;
using PrivateDataServer.Module.Request;
using PrivateDataServer.Module.Response;
using PrivateDataServer.Module.UserModule.Interface;

public class LoginService : ILoginService
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginService(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    /// <summary>
    /// ログイン処理
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.UserId, request.Password, request.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return new LoginResponse { Success = true };
        }
        return new LoginResponse { Success = false, ErrorMessage = "Invalid login attempt." };
    }

}