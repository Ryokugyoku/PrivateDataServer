namespace PrivateDataServer.Module.Request;
public class UpdateUserRequest
{
    /// <summary>
    /// ユーザID
    /// </summary>
    public string? UserId { get; set; }
    /// <summary>
    /// ユーザ名
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// メールアドレス
    /// </summary>
    public string? Email { get; set; }
}