namespace PrivateDataServer.Module.Request;
public class CreateUserRequest
{
    /// <summary>
    /// ユーザ名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// メールアドレス　※ログインする際に利用するIDとなる
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// パスワード
    /// </summary>
    public string? Password { get; set; }
}
