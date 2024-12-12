namespace PrivateDataServer.Api.UserModule.Model
{
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

    public class DeleteUserRequest
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        public string? UserId { get; set; }
    }
}