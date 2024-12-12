namespace PrivateDataServer.Api.LoginModule.Model;
public class LoginRequest
{
    /// <summary>
    /// ユーザID/メールアドレス
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// パスワード
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// ログイン状態を維持するかどうか
    /// True : 維持する
    /// False : 維持しない
    /// </summary>
    public bool RememberMe { get; set; }
}

public class LoginResult
{
    /// <summary>
    /// ログイン成功かどうか
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// エラーメッセージ
    /// </summary>
    public string ErrorMessage { get; set; }
}