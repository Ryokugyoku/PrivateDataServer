using Microsoft.AspNetCore.Identity;
using PrivateDataServer.Module.Request;

public class UserManagerService : IUserManagerService
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserManagerService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUserAsync(CreateUserRequest request)
    {
        var errors = CreateUserNullCheck(request);
        if(errors.Any()){
            return IdentityResult.Failed(errors.ToArray());
        }

        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        return result;
    }

    public async Task<IdentityResult> UpdateUserAsync(UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }
        var isUser = await CheckUserPermission(request.UserId);
        if (!isUser)
        {
            return IdentityResult.Failed(new IdentityError { Description = "You do not have permission to update this user." });
        }
        user.UserName = request.UserName;
        user.Email = request.Email;
        var result = await _userManager.UpdateAsync(user);
        return result;
    }

    /// <summary>
    /// ユーザ削除処理
    /// 自身のユーザか、Admin権限を持っている場合のみ削除可能
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IdentityResult> DeleteUserAsync(DeleteUserRequest request)
    {

    
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var isUser = await CheckUserPermission(request.UserId);

        if (!isUser)
        {
            return IdentityResult.Failed(new IdentityError { Description = "You do not have permission to delete this user." });
        }
        var result = await _userManager.DeleteAsync(user);
        return result;
    }

    private async Task<bool> CheckUserPermission(string userId)
    {
        bool isUser = false;
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HTTP context is not available.");
        }
        var currentUser = httpContext.User;
        var currentUserId = _userManager.GetUserId(currentUser);
        var currentUserIdentity = await _userManager.GetUserAsync(currentUser);
        var isAdmin = currentUserIdentity != null && await _userManager.IsInRoleAsync(currentUserIdentity, "Admin");
        if (currentUserId == userId || isAdmin)
        {
            isUser = true;
        }
        return isUser;
    }

    private List<IdentityError> CreateUserNullCheck(CreateUserRequest request)
    {
        var errors = new List<IdentityError>();

        // 入力値の検証
        if (string.IsNullOrEmpty(request.UserName))
        {
            errors.Add(new IdentityError { Description = "UserName cannot be null or empty." });
        }
        if (string.IsNullOrEmpty(request.Email))
        {
            errors.Add(new IdentityError { Description = "Email cannot be null or empty." });
        }
        if (string.IsNullOrEmpty(request.Password))
        {
            errors.Add(new IdentityError { Description = "Password cannot be null or empty." });
        }

        return errors;
    }

}