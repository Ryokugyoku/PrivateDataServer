using Microsoft.AspNetCore.Identity;
using PrivateDataServer.Module.Request;

public interface IUserManagerService
{
    Task<IdentityResult> CreateUserAsync(CreateUserRequest request);
    Task<IdentityResult> UpdateUserAsync(UpdateUserRequest request);
    Task<IdentityResult> DeleteUserAsync(DeleteUserRequest request);
}