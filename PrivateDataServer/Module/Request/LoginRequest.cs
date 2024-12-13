namespace PrivateDataServer.Module.Request;
/// <summary>
/// ログインリクエスト
/// </summary>
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