namespace PrivateDataServer.Api.LoginModule;
using PrivateDataServer.Api.LoginModule.Model;
using Microsoft.AspNetCore.Identity;

public interface ILoginService
{
    Task<LoginResult> LoginAsync(LoginRequest request);
    Task<IdentityResult> AddUserToRoleAsync(string userId, string role);
}