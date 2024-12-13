namespace PrivateDataServer.Module.Response;
/// <summary>
/// ログインレスポンス
/// </summary>
public class LoginResponse
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